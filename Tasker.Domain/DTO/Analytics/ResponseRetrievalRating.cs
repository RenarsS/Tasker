﻿namespace Tasker.Domain.DTO.Analytics;

public class ResponseRetrievalRating : Rating
{
    public int ResponseId { get; set; }

    public int TaskId { get; set; }

    public float Rating { get; set; }
    
}