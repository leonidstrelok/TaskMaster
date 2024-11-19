namespace TaskMasterAPI.Models;

public class HistoryTask : Bases.Base
{
    public string Title { get; set; }
    public string WhatHasBeenUpdated { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid TaskId { get; set; }
}