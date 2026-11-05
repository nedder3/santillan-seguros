"""Unit tests for domain models."""

from datetime import datetime, timedelta, timezone
import pytest
from pydantic import ValidationError

from src.domain.models import (
    Policy,
    PolicyType,
    PolicyStatus,
    Claim,
    ClaimStatus,
    ClaimSeverity,
    SurveyFinding,
)


def test_create_valid_policy():
    start = datetime.now(timezone.utc)
    policy = Policy(
        policy_id="POL-2026-001",
        holder_id="TAX-99887766",
        policy_type=PolicyType.AUTO,
        premium_usd=1200.0,
        coverage_limit_usd=50000.0,
        deductible_usd=500.0,
        start_date=start,
        end_date=start + timedelta(days=365),
        telemetry_enabled=True,
    )
    assert policy.policy_id == "POL-2026-001"
    assert policy.status == PolicyStatus.ACTIVE
    assert policy.telemetry_enabled is True


def test_invalid_policy_coverage_limit():
    start = datetime.now(timezone.utc)
    with pytest.raises(ValidationError):
        Policy(
            policy_id="POL-ERR",
            holder_id="TAX-000",
            policy_type=PolicyType.HOME,
            premium_usd=500.0,
            coverage_limit_usd=-1000.0,  # Invalid: gt=0 required
            deductible_usd=100.0,
            start_date=start,
            end_date=start + timedelta(days=365),
        )


def test_create_valid_claim_and_survey():
    incident_time = datetime.now(timezone.utc) - timedelta(hours=3)
    claim = Claim(
        claim_id="CLM-909",
        policy_id="POL-2026-001",
        incident_timestamp=incident_time,
        location="Av. Corrientes 1234, CABA",
        description="Rear-end collision at intersection while stationary.",
        claimed_amount_usd=4500.0,
        severity=ClaimSeverity.LOW,
    )
    assert claim.status == ClaimStatus.REPORTED

    survey = SurveyFinding(
        survey_id="SRV-101",
        claim_id=claim.claim_id,
        adjuster_id="ADJ-44",
        verified_damage_usd=4200.0,
        is_total_loss=False,
        certified_evidence_hashes=["e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855"],
        notes="Bumper and trunk lid replacement approved.",
    )
    assert survey.verified_damage_usd == 4200.0
    assert not survey.is_total_loss
