"""Unit tests for the loss prevention sentinel."""

from datetime import datetime
import pytest

from src.engine.prevention_sentinel import (
    PreventionSentinel,
    TelemetryEvent,
    SensorType,
    AlertAction,
)


def test_sub_threshold_event_ignored():
    sentinel = PreventionSentinel()
    event = TelemetryEvent(
        event_id="EVT-001",
        device_id="DEV-WATER-99",
        policy_id="POL-HOME-400",
        sensor_type=SensorType.WATER_FLOW,
        measured_value=12.5,  # Threshold is 45.0
        unit="L/min",
    )
    alert = sentinel.process_event(event)
    assert alert is None


def test_critical_water_leak_triggers_automated_shutoff():
    sentinel = PreventionSentinel()
    event = TelemetryEvent(
        event_id="EVT-CRITICAL-LEAK",
        device_id="DEV-WATER-99",
        policy_id="POL-HOME-400",
        sensor_type=SensorType.WATER_FLOW,
        measured_value=58.2,  # Exceeds 45.0
        unit="L/min",
    )
    alert = sentinel.process_event(event)
    assert alert is not None
    assert alert.severity == "CRITICAL"
    assert alert.mitigation_action == AlertAction.AUTOMATED_SHUTOFF_COMMAND
    assert alert.incident_prevented_estimate_usd == 15000.0


def test_industrial_vibration_escalation():
    sentinel = PreventionSentinel()
    event = TelemetryEvent(
        event_id="EVT-IND-TURBINE-01",
        device_id="DEV-VIB-01",
        policy_id="POL-IND-880",
        sensor_type=SensorType.INDUSTRIAL_VIBRATION,
        measured_value=11.2,  # Limit is 8.5
        unit="mm/s",
    )
    alert = sentinel.process_event(event)
    assert alert is not None
    assert alert.mitigation_action == AlertAction.AUTOMATED_SHUTOFF_COMMAND
    assert alert.incident_prevented_estimate_usd == 120000.0
