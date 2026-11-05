"""Unit tests for the fraud heuristic engine."""

from datetime import datetime, timedelta
import pytest

from src.domain.models import Policy, PolicyType, Claim, ClaimSeverity
from src.engine.fraud_detector import FraudDetectionEngine, FraudRiskLevel


@pytest.fixture
def base_policy():
    start = datetime(2026, 1, 1, 0, 0, 0)
    return Policy(
        policy_id="POL-TEST-001",
        holder_id="HOLDER-123",
        policy_type=PolicyType.AUTO,
        premium_usd=1500.0,
        coverage_limit_usd=100000.0,
        deductible_usd=1000.0,
        start_date=start,
        end_date=start + timedelta(days=365),
    )


def test_standard_claim_low_fraud_score(base_policy):
    engine = FraudDetectionEngine(siu_threshold=0.60)
    incident_time = base_policy.start_date + timedelta(days=90)
    claim = Claim(
        claim_id="CLM-001",
        policy_id=base_policy.policy_id,
        incident_timestamp=incident_time,
        location="Calle Falsa 123",
        description="Fender bender in shopping parking lot.",
        claimed_amount_usd=3000.0,
        severity=ClaimSeverity.LOW,
    )

    result = engine.evaluate_claim(claim, base_policy, prior_claims_count=0, is_night_incident=False)
    assert result.risk_score == 0.0
    assert result.risk_level == FraudRiskLevel.LOW
    assert not result.requires_siu_investigation
    assert len(result.triggered_rules) == 0


def test_rapid_inception_and_near_limit_triggers_siu(base_policy):
    engine = FraudDetectionEngine(siu_threshold=0.60)
    # Incident 3 days after issuance
    incident_time = base_policy.start_date + timedelta(days=3)
    # 95% of coverage limit claimed ($95,000 of $100,000)
    claim = Claim(
        claim_id="CLM-SUSPICIOUS-01",
        policy_id=base_policy.policy_id,
        incident_timestamp=incident_time,
        location="Rural Route 9, Km 42",
        description="Total vehicle burn out with no eyewitnesses.",
        claimed_amount_usd=95000.0,
        severity=ClaimSeverity.HIGH,
    )

    result = engine.evaluate_claim(claim, base_policy, prior_claims_count=0, is_night_incident=False)
    # Expected: Rapid Inception (+0.35) + Near Limit (+0.25) = 0.60
    assert result.risk_score >= 0.60
    assert result.requires_siu_investigation is True
    assert result.risk_level in [FraudRiskLevel.HIGH, FraudRiskLevel.CRITICAL]
    assert len(result.triggered_rules) == 2


def test_night_high_value_and_recurrence(base_policy):
    engine = FraudDetectionEngine(siu_threshold=0.60)
    incident_time = base_policy.start_date + timedelta(days=120)
    claim = Claim(
        claim_id="CLM-NIGHT-02",
        policy_id=base_policy.policy_id,
        incident_timestamp=incident_time,
        location="Industrial Warehouse dock",
        description="Equipment theft overnight.",
        claimed_amount_usd=40000.0,
        severity=ClaimSeverity.HIGH,
    )

    result = engine.evaluate_claim(claim, base_policy, prior_claims_count=2, is_night_incident=True)
    # Off hours high value (+0.20) + Recurrence 2 prior (+0.30) = 0.50
    assert result.risk_score == 0.50
    assert result.risk_level == FraudRiskLevel.HIGH
    assert result.requires_siu_investigation is False  # Below 0.60 threshold
