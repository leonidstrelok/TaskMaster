﻿using TaskMasterAPI.Models.Bases;
using TaskMasterAPI.Models.Clients;

namespace TaskMasterAPI.Models;

public class Task : Bases.Base
{
    public string Title { get; set; }
    public string Description { get; set; }
    public StatusType Status { get; set; }
    public DateTime ExpiredAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public PriorityType Priority { get; set; }
    public string ClientId { get; set; }
    public Client Client { get; set; }
    public Guid? HistoryTaskId { get; set; }
    public ICollection<CommentTask> Comments { get; set; }
    public LabelType Label { get; set; }
}