"""Proactive loss prevention sentinel and IoT telemetry triage engine."""

from datetime import datetime, timezone
from enum import Enum
from typing import List, Optional
from pydantic import BaseModel, Field


def _utcnow() -> datetime:
    return datetime.now(timezone.utc)


class SensorType(str, Enum):
    TELEMATICS_SPEED = "TELEMATICS_SPEED"
    WATER_FLOW = "WATER_FLOW"
    SMOKE_DETECTOR = "SMOKE_DETECTOR"
    TEMPERATURE = "TEMPERATURE"
    INDUSTRIAL_VIBRATION = "INDUSTRIAL_VIBRATION"


class AlertAction(str, Enum):
    NOTIFY_POLICYHOLDER = "NOTIFY_POLICYHOLDER"
    AUTOMATED_SHUTOFF_COMMAND = "AUTOMATED_SHUTOFF_COMMAND"
    DISPATCH_PREVENTION_CREW = "DISPATCH_PREVENTION_CREW"
    RATE_SURCHARGE_FLAG = "RATE_SURCHARGE_FLAG"


class TelemetryEvent(BaseModel):
    event_id: str
    device_id: str
    policy_id: str
    sensor_type: SensorType
    measured_value: float
    unit: str
    timestamp: datetime = Field(default_factory=_utcnow)


class PreventionAlert(BaseModel):
    alert_id: str
    policy_id: str
    sensor_type: SensorType
    severity: str
    mitigation_action: AlertAction
    incident_prevented_estimate_usd: float = Field(..., ge=0)
    details: str


class PreventionSentinel:
    """Monitors live telemetry feeds to mitigate losses before an insurance claim occurs."""

    THRESHOLDS = {
        SensorType.WATER_FLOW: {"critical": 45.0, "action": AlertAction.AUTOMATED_SHUTOFF_COMMAND, "loss_saved": 15000.0},
        SensorType.TEMPERATURE: {"critical": 65.0, "action": AlertAction.DISPATCH_PREVENTION_CREW, "loss_saved": 85000.0},
        SensorType.TELEMATICS_SPEED: {"critical": 140.0, "action": AlertAction.NOTIFY_POLICYHOLDER, "loss_saved": 22000.0},
        SensorType.INDUSTRIAL_VIBRATION: {"critical": 8.5, "action": AlertAction.AUTOMATED_SHUTOFF_COMMAND, "loss_saved": 120000.0},
    }

    def process_event(self, event: TelemetryEvent) -> Optional[PreventionAlert]:
        cfg = self.THRESHOLDS.get(event.sensor_type)
        if not cfg:
            return None

        if event.measured_value >= cfg["critical"]:
            return PreventionAlert(
                alert_id=f"ALT-{event.event_id[-8:]}",
                policy_id=event.policy_id,
                sensor_type=event.sensor_type,
                severity="CRITICAL",
                mitigation_action=cfg["action"],
                incident_prevented_estimate_usd=cfg["loss_saved"],
                details=(
                    f"Threshold exceeded for {event.sensor_type.value}: "
                    f"measured {event.measured_value} {event.unit} >= {cfg['critical']} limit."
                ),
            )
        return None
