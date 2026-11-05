"""Heuristic and anomaly fraud scoring engine for insurance claims."""

from datetime import datetime, timedelta
from enum import Enum
from typing import List, Dict, Any, Optional
from pydantic import BaseModel, Field

from ..domain.models import Claim, Policy, ClaimStatus


class FraudRiskLevel(str, Enum):
    LOW = "LOW"
    MEDIUM = "MEDIUM"
    HIGH = "HIGH"
    CRITICAL = "CRITICAL"


class FraudEvaluationResult(BaseModel):
    claim_id: str
    risk_score: float = Field(..., ge=0.0, le=1.0, description="Normalized fraud risk probability between 0.0 and 1.0")
    risk_level: FraudRiskLevel
    triggered_rules: List[str]
    requires_siu_investigation: bool = Field(
        ..., description="True when routed to the Special Investigation Unit (SIU)"
    )
    explanation: str


class FraudDetectionEngine:
    """Evaluates incoming claims against deterministic actuarial and behavioral heuristics."""

    def __init__(self, siu_threshold: float = 0.60):
        self.siu_threshold = siu_threshold

    def evaluate_claim(
        self,
        claim: Claim,
        policy: Policy,
        prior_claims_count: int = 0,
        is_night_incident: bool = False,
    ) -> FraudEvaluationResult:
        score = 0.0
        triggered: List[str] = []

        # Rule 1: Rapid Inception Claim (< 15 days from start date)
        policy_age_days = (claim.incident_timestamp - policy.start_date).total_seconds() / 86400.0
        if policy_age_days < 15:
            score += 0.35
            triggered.append(f"RAPID_INCEPTION_CLAIM: Incident occurred {policy_age_days:.1f} days after policy issuance (+0.35)")

        # Rule 2: Near-Limit Loss Ratio (> 90% of coverage limit)
        loss_ratio = claim.claimed_amount_usd / policy.coverage_limit_usd
        if loss_ratio >= 0.90:
            score += 0.25
            triggered.append(f"HIGH_LIMIT_EXPOSURE: Claimed amount represents {loss_ratio * 100:.1f}% of maximum policy limit (+0.25)")

        # Rule 3: High-Value Incident during Off-Hours / Night
        if is_night_incident and claim.claimed_amount_usd > 25000:
            score += 0.20
            triggered.append(f"OFF_HOURS_HIGH_VALUE: Night incident exceeding $25,000 threshold (+0.20)")

        # Rule 4: Recurrent Claimant Frequency
        if prior_claims_count >= 2:
            recurrence_weight = min(0.30, prior_claims_count * 0.15)
            score += recurrence_weight
            triggered.append(f"RECURRENT_CLAIM_HISTORY: {prior_claims_count} prior claims logged within observation window (+{recurrence_weight:.2f})")

        normalized_score = min(1.0, round(score, 3))

        if normalized_score >= 0.75:
            level = FraudRiskLevel.CRITICAL
        elif normalized_score >= 0.50:
            level = FraudRiskLevel.HIGH
        elif normalized_score >= 0.25:
            level = FraudRiskLevel.MEDIUM
        else:
            level = FraudRiskLevel.LOW

        requires_siu = normalized_score >= self.siu_threshold

        explanation = (
            f"Evaluated with {len(triggered)} rule triggers. "
            f"Score: {normalized_score:.3f}. Severity: {level.value}."
        )

        return FraudEvaluationResult(
            claim_id=claim.claim_id,
            risk_score=normalized_score,
            risk_level=level,
            triggered_rules=triggered,
            requires_siu_investigation=requires_siu,
            explanation=explanation,
        )
