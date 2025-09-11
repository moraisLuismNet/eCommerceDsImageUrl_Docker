using AutoMapper;
using eCommerceDs.DTOs;
using eCommerceDs.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceDs.Controllers
{
    /// <summary>
    /// Controller for managing music groups
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class GroupsController : ControllerBase
    {
        private readonly IValidator<GroupInsertDTO> _groupInsertValidator;
        private readonly IValidator<GroupUpdateDTO> _groupUpdateValidator;
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Group controller constructor
        /// </summary>
        public GroupsController(
            IValidator<GroupInsertDTO> groupInsertValidator,
            IValidator<GroupUpdateDTO> groupUpdateValidator,
            IGroupService groupService, 
            IMapper mapper)
        {
            _groupInsertValidator = groupInsertValidator;
            _groupUpdateValidator = groupUpdateValidator;
            _groupService = groupService;
            _mapper = mapper;
        }

        /// <summary>
        /// Create a new music group
        /// </summary>
        /// <param name="groupInsertDTO">Details of the group to be created</param>
        /// <returns>The group has been created with its assigned ID.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GroupItemDTO>> Add(GroupInsertDTO groupInsertDTO)
        {
            try
            {
                // Validate the model
                var validationResult = await _groupInsertValidator.ValidateAsync(groupInsertDTO);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new 
                    { 
                        message = "Validation error",
                        errors = validationResult.Errors.Select(e => e.ErrorMessage)
                    });
                }

                // Create the group
                var groupDTO = await _groupService.AddService(groupInsertDTO);
                var groupItemDTO = _mapper.Map<GroupItemDTO>(groupDTO);

                return CreatedAtAction(
                    nameof(GetById), 
                    new { IdGroup = groupItemDTO.IdGroup }, 
                    new 
                    {
                        message = "Group created successfully",
                        data = groupItemDTO
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new 
                { 
                    message = "Error creating the group",
                    detalle = ex.Message
                });
            }
        }

        /// <summary>
        /// Get all the music groups
        /// </summary>
        /// <returns>List of all groups</returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GroupDTO>>> Get()
        {
            var groups = await _groupService.GetService();
            return Ok(groups);
        }

        

        /// <summary>
        /// Get a group by its ID
        /// </summary>
        /// <param name="IdGroup">Group ID to search for</param>
        /// <returns>The found group, or a 404 error if not found.</returns>
        [HttpGet("{IdGroup:int}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GroupItemDTO>> GetById(int IdGroup)
        {
            if (IdGroup <= 0)
            {
                return BadRequest("The group ID must be greater than zero");
            }

            var groupDTO = await _groupService.GetByIdService(IdGroup);
            if (groupDTO == null)
            {
                return NotFound($"The group with ID {IdGroup} was not found");
            }

            var groupItemDTO = _mapper.Map<GroupItemDTO>(groupDTO);
            return Ok(groupItemDTO);
        }


        /// <summary>
        /// Retrieves all groups along with their associated albums
        /// </summary>
        /// <returns>List of bands with their albums</returns>
        [HttpGet("groupsRecords")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<GroupRecordsDTO>>> GetGroupsRecords()
        {
            try
            {
                var groupRecords = await _groupService.GetGroupsRecordsGroupService();
                return Ok(groupRecords);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new 
                { 
                    message = "Error while retrieving the groups and their associated albums",
                    detalle = ex.Message
                });
            }
        }


        /// <summary>
        /// Retrieves a specific group along with all its associated discs
        /// </summary>
        /// <param name="idGroup">Group ID to search for</param>
        /// <returns>The band with its associated albums, or a 404 error if not found.</returns>
        [HttpGet("recordsByGroup/{idGroup}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GroupRecordsDTO>> GetRecordsByGroup(int idGroup)
        {
            if (idGroup <= 0)
            {
                return BadRequest("The group ID must be greater than zero");
            }

            try
            {
                var groupDTO = await _groupService.GetRecordsByGroupGroupService(idGroup);

                if (groupDTO == null)
                {
                    return NotFound($"The group with ID {idGroup} was not found.");
                }

                return Ok(groupDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new 
                { 
                    message = $"Error while retrieving the disks for the group with ID {idGroup}",
                    detalle = ex.Message
                });
            }
        }


        /// <summary>
        /// Search for groups by name
        /// </summary>
        /// <param name="text">Text to search in group names</param>
        /// <returns>List of groups that match the search text</returns>
        [HttpGet("SearchByName/{text}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<GroupItemDTO>>> SearchByName(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return BadRequest("The search text cannot be empty");
            }

            if (text.Length < 2)
            {
                return BadRequest("The search text must have at least 2 characters");
            }

            try
            {
                var groups = await _groupService.SearchByNameGroupService(text);

                if (groups == null || !groups.Any())
                {
                    return NotFound($"No groups found with the name '{text}'");
                }

                var groupsItem = _mapper.Map<IEnumerable<GroupItemDTO>>(groups);
                return Ok(groupsItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new 
                { 
                    message = "Error while searching for groups by name",
                    detalle = ex.Message
                });
            }
        }


        /// <summary>
        /// Get groups sorted by name
        /// </summary>
        /// <param name="ascen">True for ascending order, False for descending order</param>
        /// <returns>List of groups sorted by name</returns>
        [HttpGet("sortedByName/{ascen}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<GroupItemDTO>>> GetSortedByName(bool ascen)
        {
            try
            {
                var groups = await _groupService.GetSortedByNameGroupService(ascen);
                var groupsItem = _mapper.Map<IEnumerable<GroupItemDTO>>(groups);
                
                return Ok(groupsItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new 
                { 
                    message = "Error while getting groups sorted by name",
                    detalle = ex.Message
                });
            }
        }


        /// <summary>
        /// Update an existing group
        /// </summary>
        /// <param name="IdGroup">ID of the group to update</param>
        /// <param name="groupUpdateDTO">Updated group data</param>
        /// <returns>The updated group or an error message</returns>
        [HttpPut("{IdGroup:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GroupItemDTO>> Update(int IdGroup, [FromBody] GroupUpdateDTO groupUpdateDTO)
        {
            if (IdGroup <= 0)
            {
                return BadRequest("The group ID must be greater than zero");
            }

            try
            {
                // Validate the model
                var validationResult = await _groupUpdateValidator.ValidateAsync(groupUpdateDTO);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new 
                    { 
                        message = "Validation error",
                        errors = validationResult.Errors.Select(e => e.ErrorMessage)
                    });
                }

                // Verify that the ID in the route matches the ID in the DTO
                if (IdGroup != groupUpdateDTO.IdGroup)
                {
                    return BadRequest("The ID in the route does not match the ID in the DTO");
                }

                // Update the group
                var groupDTO = await _groupService.UpdateService(IdGroup, groupUpdateDTO);
                
                if (groupDTO == null)
                {
                    return NotFound(new 
                    { 
                        message = $"The group with ID {IdGroup} was not found"
                    });
                }

                var groupItemDTO = _mapper.Map<GroupItemDTO>(groupDTO);
                return Ok(new 
                { 
                    message = "Group updated successfully",
                    data = groupItemDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new 
                { 
                    message = $"Error updating the group with ID {IdGroup}",
                    detalle = ex.Message
                });
            }
        }

        /// <summary>
        /// Delete a group by its ID
        /// </summary>
        /// <param name="IdGroup">Group ID to delete</param>
        /// <returns>The deleted group or an error message</returns>
        [HttpDelete("{IdGroup:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GroupItemDTO>> Delete(int IdGroup)
        {
            if (IdGroup <= 0)
            {
                return BadRequest("The group ID must be greater than zero");
            }

            try
            {
                // Verify if the group has associated records
                bool hasGroups = await _groupService.GroupHasRecordsGroupService(IdGroup);
                if (hasGroups)
                {
                    return BadRequest(new 
                    { 
                        message = $"The group with ID {IdGroup} cannot be deleted",
                        detalle = "The group has associated records"
                    });
                }

                // Delete the group
                var groupDTO = await _groupService.DeleteService(IdGroup);
                
                if (groupDTO == null)
                {
                    return NotFound(new 
                    { 
                        message = $"The group with ID {IdGroup} was not found"
                    });
                }

                var groupItemDTO = _mapper.Map<GroupItemDTO>(groupDTO);
                return Ok(new 
                { 
                    message = "Group deleted successfully",
                    data = groupItemDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new 
                { 
                    message = $"Error deleting the group with ID {IdGroup}",
                    detalle = ex.Message
                });
            }
        }
    }
}
