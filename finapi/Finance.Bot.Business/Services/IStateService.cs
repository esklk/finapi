﻿using Finance.Bot.Business.Models;

namespace Finance.Bot.Business.Services
{
    public interface IStateService
    {
        Task<State> GetStateAsync();

        Task SetStateAsync(State state);
    }
}
