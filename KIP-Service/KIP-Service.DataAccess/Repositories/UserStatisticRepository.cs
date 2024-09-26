﻿using CSharpFunctionalExtensions;
using KIP_Service.Core.Models;
using KIP_Service.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace KIP_Service.DataAccess.Repositories
{
    public class UserStatisticRepository(
        KIP_ServiceDbContext context,
        IConfiguration configuration,
        ILogger<UserStatisticRepository> logger) : IUserStatisticRepository
    {
        private readonly KIP_ServiceDbContext _context = context;
        private readonly ILogger<UserStatisticRepository> _logger = logger;

        public async Task<Result<UserStatistic>> GetAsync(RequestStatistic requestStatistic)
        {
            List<UserSingInEntity> userSingInsEntity;

            try
            {
                userSingInsEntity = await _context.UserSingIns
                    .AsNoTracking()
                    .Where(u => u.UserId == requestStatistic.UserId &&
                                u.SingInDate >= requestStatistic.From &&
                                u.SingInDate <= requestStatistic.To)
                    .ToListAsync();
            }
            catch (ObjectDisposedException)
            {
                using (var context = new KIP_ServiceDbContext(configuration))
                {
                    userSingInsEntity = await context.UserSingIns
                        .AsNoTracking()
                        .Where(u => u.UserId == requestStatistic.UserId &&
                                    u.SingInDate >= requestStatistic.From &&
                                    u.SingInDate <= requestStatistic.To)
                        .ToListAsync();
                }
            }

            return new UserStatistic(
                requestStatistic.UserId,
                userSingInsEntity.Count);
        }

        public async Task<Result> AddSinginAsync(UserSingIn userSingIn)
        {
            try
            {
                var userSingInEntity = new UserSingInEntity
                {
                    Id = userSingIn.Id,
                    UserId = userSingIn.UserId,
                    SingInDate = userSingIn.SingInDate,
                };

                await _context.UserSingIns.AddAsync(userSingInEntity);
                await _context.SaveChangesAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return Result.Failure(ex.Message);
            }
        }
    }
}
