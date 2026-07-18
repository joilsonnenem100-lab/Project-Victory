using System;
using ProjectVictory.Domain.Playtest;

namespace ProjectVictory.Application.Playtest;

public sealed record ClassifyPlaytestIssueCommand(
    Guid FeedbackId,
    PlaytestIssueClassification Classification);