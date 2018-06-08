CREATE TABLE [jcu].[EventForm]
(
	[EventId] UNIQUEIDENTIFIER NOT NULL,
	[FormId] UNIQUEIDENTIFIER NOT NULL, 
    [CreateTimestamp] DATETIME NOT NULL DEFAULT GetDate(), 
    [UpdateTimestamp] DATETIME NOT NULL DEFAULT GetDate(), 
    CONSTRAINT [PK_EventForm] PRIMARY KEY ([EventId], [FormId]), 
    CONSTRAINT [FK_EventForm_To_Event] FOREIGN KEY ([EventId]) REFERENCES [jcu].[Event]([EventId]), 
    CONSTRAINT [FK_EventForm_To_Form] FOREIGN KEY ([FormId]) REFERENCES [jcu].[Form]([FormId])
)

GO

CREATE INDEX [IX_EventForm_EventId] ON [jcu].[EventForm] ([EventId])

GO

CREATE INDEX [IX_EventForm_FormId] ON [jcu].[EventForm] ([FormId])
