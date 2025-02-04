﻿using Tasker.API.Services.Interfaces;
using Tasker.Domain.MasterData;
using Tasker.Infrastructure.Repositories.Interfaces;

namespace Tasker.API.Services;

public class TaskTypeService(ITaskTypeRepository statusRepository) : ITaskTypeService
{
    public async Task<IEnumerable<TaskType>> GetAllTaskTypes()
    {
        return await statusRepository.GetTaskTypes();
    }

    public async Task<TaskType> GetTaskTypeById(int id)
    {
        return await statusRepository.GetTaskTypeById(id);
    }
}