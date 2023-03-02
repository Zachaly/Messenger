﻿namespace Messenger.Models.Response
{
    public class ResponseModel
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public IDictionary<string, string[]>? Errors { get; set; }
        public long? NewEntityId { get; set; }
    }
}
