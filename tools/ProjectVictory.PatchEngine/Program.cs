using ProjectVictory.PatchEngine.Commands;

var dispatcher = new CommandDispatcher();

dispatcher.Register(new PreviewCommand());
dispatcher.Register(new ApplyCommand());
dispatcher.Register(new RollbackCommand());
dispatcher.Register(new GenerateCommand());

return dispatcher.Dispatch(args);