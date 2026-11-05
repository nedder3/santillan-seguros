"""Core domain models for Federación Santillán Insurance & Loss Prevention System."""

from datetime import datetime, timezone
from enum import Enum
from typing import Optional, Dict, Any, List
from pydantic import BaseModel, Field


def _utcnow() -> datetime:
    return datetime.now(timezone.utc)


class PolicyType(str, Enum):
    AUTO = "AUTO"
    HOME = "HOME"
    LIFE = "LIFE"
    COMMERCIAL = "COMMERCIAL"
    WORKERS_COMP = "WORKERS_COMP"


class PolicyStatus(str, Enum):
    ACTIVE = "ACTIVE"
    SUSPENDED = "SUSPENDED"
    EXPIRED = "EXPIRED"
    CANCELLED = "CANCELLED"


class Policy(BaseModel):
    policy_id: str = Field(..., description="Unique alphanumeric policy identifier (e.g., POL-2026-901)")
    holder_id: str = Field(..., description="Insured party tax or identification number")
    policy_type: PolicyType = Field(..., description="Insurance line of business")
    premium_usd: float = Field(..., ge=0, description="Annualized policy premium in USD")
    coverage_limit_usd: float = Field(..., gt=0, description="Maximum insured value")
    deductible_usd: float = Field(..., ge=0, description="Deductible payable by policyholder upon claim")
    start_date: datetime
    end_date: datetime
    status: PolicyStatus = Field(default=PolicyStatus.ACTIVE)
    telemetry_enabled: bool = Field(default=False, description="Whether active IoT / prevention sensor stream is linked")


class ClaimSeverity(str, Enum):
    LOW = "LOW"
    MEDIUM = "MEDIUM"
    HIGH = "HIGH"
    CATASTROPHIC = "CATASTROPHIC"


class ClaimStatus(str, Enum):
    REPORTED = "REPORTED"
    UNDER_SURVEY = "UNDER_SURVEY"
    PENDING_TRIAGE = "PENDING_TRIAGE"
    FRAUD_FLAGGED = "FRAUD_FLAGGED"
    APPROVED = "APPROVED"
    SETTLED = "SETTLED"
    REJECTED = "REJECTED"


class Claim(BaseModel):
    claim_id: str = Field(..., description="Unique claim identifier (e.g., CLM-2026-004)")
    policy_id: str = Field(..., description="Associated policy reference")
    incident_timestamp: datetime
    reported_timestamp: datetime = Field(default_factory=_utcnow)
    location: str = Field(..., description="Physical coordinates or standardized geocoded address")
    description: str = Field(..., description="Incident narrative provided by the claimant")
    claimed_amount_usd: float = Field(..., gt=0, description="Estimated total loss claimed")
    status: ClaimStatus = Field(default=ClaimStatus.REPORTED)
    severity: ClaimSeverity = Field(default=ClaimSeverity.MEDIUM)


class SurveyFinding(BaseModel):
    survey_id: str = Field(..., description="Expert surveyor dispatch record ID")
    claim_id: str = Field(..., description="Target claim reference")
    adjuster_id: str = Field(..., description="Certified loss adjuster ID")
    inspection_timestamp: datetime = Field(default_factory=_utcnow)
    verified_damage_usd: float = Field(..., ge=0, description="Adjusted and verified repair/loss estimate")
    is_total_loss: bool = Field(default=False)
    certified_evidence_hashes: List[str] = Field(default_factory=list, description="SHA-256 digests of on-site photo/video evidence")
    notes: str = Field(default="", description="Technical adjuster remarks")
