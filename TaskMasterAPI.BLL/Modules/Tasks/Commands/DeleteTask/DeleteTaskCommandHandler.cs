using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskMasterAPI.DAL.Enums;
using TaskMasterAPI.DAL.Interfaces;

namespace TaskMasterAPI.BLL.Modules.Tasks.Commands.DeleteTask;

public class DeleteTaskCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<DeleteTaskCommand>
{
    public async Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await dbContext.Set<Models.Task>()
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);
        
        await dbContext.SaveChangesAsync(task, SaveChangeType.Delete, cancellationToken);
    }
}