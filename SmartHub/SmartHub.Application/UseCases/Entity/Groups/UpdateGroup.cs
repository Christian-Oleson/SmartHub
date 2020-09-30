﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartHub.Application.Common.Interfaces.Database;
using SmartHub.Application.Common.Models;
using SmartHub.Domain.Common;

namespace SmartHub.Application.UseCases.Entity.Groups
{
    public class GroupUpdateCommand : IRequest<Response<GroupDto>>
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        // public List<DeviceDto> DeviceDtos { get; set; }

        public GroupUpdateCommand(string? description, string? name, string id)
        {
            Description = description;
            Name = name;
            Id = id;
            // DeviceDtos = deviceDtos;
        }
    }

    public class GroupUpdateHandler : IRequestHandler<GroupUpdateCommand, Response<GroupDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GroupUpdateHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<GroupDto>> Handle(GroupUpdateCommand request, CancellationToken cancellationToken)
        {
            var home = await _unitOfWork.HomeRepository.GetHome();
            if (home == null)
            {
                return Response.Fail<GroupDto>("Error: No home created yet.");
            }

            if (request.Name == DefaultNames.DefaultGroup)
            {
                return Response.Fail<GroupDto>("Error: You can't rename the default group.");
            }

            var result = home.UpdateGroup(request.Id, request.Name, request.Description);
            if (result)
            {
                Response.Fail<GroupDto>($"Error: Couldn't update group with id {request.Id}.");
            }
            return Response.Ok<GroupDto>($"Updated group with name {request.Name}", null);
        }
    }
}