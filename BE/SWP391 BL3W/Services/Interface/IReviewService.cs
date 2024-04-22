﻿using SWP391_BL3W.DTO.Request;
using SWP391_BL3W.DTO.Response;

namespace SWP391_BL3W.Services.Interface
{
    public interface IReviewService
    {
        public Task<StatusResponse<ReviewUserResponseDTO>> create(ReviewUserRequestDTO dto);
    }
}
