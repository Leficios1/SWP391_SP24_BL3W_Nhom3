﻿using SWP391_BL3W.DTO.Request;
using SWP391_BL3W.DTO.Response;

namespace SWP391_BL3W.Services.Interface
{
    public interface IAuthService
    {
        public Task<StatusResponse<TokenResponse>> LoginAccount(AuthRequestDTO dto);
        public Task<UserByTokenResponse> GetUserByToken(string token);
    }
}
