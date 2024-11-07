﻿using Microsoft.AspNetCore.Mvc;
using MovieService.DTO;
using MovieService.Helper;
using MovieService.Service.Interface;

namespace MovieService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieScheduleController : ControllerBase
{
    private readonly IMovieScheduleService _movieScheduleService;
    
    public MovieScheduleController(IMovieScheduleService movieScheduleService)
    {
        _movieScheduleService = movieScheduleService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        //lấy ra các param phân trang
        var page = int.Parse(Request.Query["page"]);
        var limit = int.Parse(Request.Query["limit"]);
        //in thử các param phân trang
        // Console.WriteLine($"Page: {page}");
        // Console.WriteLine($"Limit: {limit}");
        
        var result = await _movieScheduleService.GetAllAsync(page, limit);
        
        //chỉnh sửa timezone
        foreach (var movieSchedule in (List<MovieScheduleDTO>)result["records"])
        {
            movieSchedule.CreatedAt = movieSchedule.CreatedAt.HasValue
                ? TimeZoneHelper.ConvertToTimeZone(movieSchedule.CreatedAt.Value)
                : (DateTime?)null;
            
            movieSchedule.ShowTime = movieSchedule.ShowTime.HasValue
                ? TimeZoneHelper.ConvertToTimeZone(movieSchedule.ShowTime.Value)
                : (DateTime?)null;
        }
        
        return Ok(result);
    }
    
    //lấy lịch chiếu theo id phim
    [HttpGet]
    [Route("movie/{movieId}")]
    public async Task<IActionResult> GetByMovieIdAsync(string movieId)
    {
        var result = await _movieScheduleService.GetByMovieIdAsync(movieId);
        
        //chỉnh sửa timezone
        foreach (var movieSchedule in result)
        {
            movieSchedule.CreatedAt = movieSchedule.CreatedAt.HasValue
                ? TimeZoneHelper.ConvertToTimeZone(movieSchedule.CreatedAt.Value)
                : (DateTime?)null;
            
            movieSchedule.ShowTime = movieSchedule.ShowTime.HasValue
                ? TimeZoneHelper.ConvertToTimeZone(movieSchedule.ShowTime.Value)
                : (DateTime?)null;
        }
        
        return Ok(result);
    }
    
    //get by id
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        var result = await _movieScheduleService.GetByIdAsync(id);
        
        //chỉnh sửa timezone
        
        
        return Ok(result);
    }
    
    //lấy theo ngày chiếu. Ví dụ: GET /api/MovieSchedule/showDates?showDates=2024-06-15&showDates=2024-06-16
    [HttpGet]
    [Route("showDates")]
    public async Task<IActionResult> GetByShowDatesAsync()
    {
        //lấy param từ Request.QueryString
        var showDateStrings = Request.Query["showDates"].ToList();
        
        // nếu không có param nào
        if (showDateStrings.Count == 0)
        {
            return BadRequest("No dates were provided.");
        }
        
        var showDates = new List<DateTime>();

        foreach (var dateString in showDateStrings)
        {
            if (DateTime.TryParse(dateString, out var date))
            {
                showDates.Add(date);
            }
            else
            {
                return BadRequest($"Invalid date format: '{dateString}'.");
            }
        }
        
        var result = await _movieScheduleService.GetByShowDatesAsync(showDates);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] SaveMovieScheduleDTO movieScheduleDto)
    {
        var result = await _movieScheduleService.AddAsync(movieScheduleDto);
        return Ok(result);
    }
    
    // [HttpPost]
    // [Route("list")]
    // public async Task<IActionResult> AddListAsync([FromBody] List<SaveMovieScheduleDTO> movieScheduleDtos)
    // {
    //     var result = await _movieScheduleService.AddListAsync(movieScheduleDtos);
    //     return Ok(result);
    // }
    
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] SaveMovieScheduleDTO movieScheduleDto)
    {
        var result = await _movieScheduleService.UpdateAsync(id, movieScheduleDto);
        return Ok(result);
    }
    
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> RemoveAsync(string id)
    {
        var result = await _movieScheduleService.RemoveAsync(id);
        return Ok(result);
    }
    
    //xóa tất cả movie schedule
    [HttpDelete]
    [Route("deleteAll")]
    public async Task<IActionResult> DeleteAllAsync()
    {
        await _movieScheduleService.DeleteAll();
        return Ok();
    }
    
}