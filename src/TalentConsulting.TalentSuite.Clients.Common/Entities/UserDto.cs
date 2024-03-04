﻿using System.Diagnostics.CodeAnalysis;

namespace TalentConsulting.TalentSuite.Clients.Common.Entities;

[ExcludeFromCodeCoverage]
public record UserDto
{
    private UserDto() { }

    public UserDto(string id, string firstname, string lastname, string email, string usergroupid, ICollection<ReportDto> reports)
    {
        Id = id;
        Firstname = firstname;
        Lastname = lastname;
        Email = email;
        UserGroupId = usergroupid;
        Reports = reports;
    }

    public string Id { get; init; } = default!;
    public string Firstname { get; init; } = default!;
    public string Lastname { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string UserGroupId { get; init; } = default!;
    public ICollection<ReportDto> Reports { get; init; } = new List<ReportDto>();
}