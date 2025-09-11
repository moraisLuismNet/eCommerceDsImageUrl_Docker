using AutoMapper;
using eCommerceDs.DTOs;
using eCommerceDs.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceDs.Controllers
{
    /// <summary>
    /// Controller for managing records
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RecordsController : ControllerBase
    {
        private readonly IValidator<RecordInsertDTO> _recordInsertValidator;
        private readonly IValidator<RecordUpdateDTO> _recordUpdateValidator;
        private readonly IRecordService _recordService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Records controller constructor
        /// </summary>
        public RecordsController(
            IValidator<RecordInsertDTO> recordInsertValidator, 
            IValidator<RecordUpdateDTO> recordUpdateValidator, 
            IRecordService recordService,
            IMapper mapper)
        {
            _recordInsertValidator = recordInsertValidator;
            _recordUpdateValidator = recordUpdateValidator;
            _recordService = recordService;
            _mapper = mapper;
        }


        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns>A list of all records</returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RecordDTO>>> Get()
        {
            var records = await _recordService.GetService();
            return Ok(records);
        }



        /// <summary>
        /// Get a record by its ID
        /// </summary>
        /// <param name="IdRecord">ID of the record to search for</param>
        /// <returns>The record found or a 404 error if not found</returns>
        [HttpGet("{IdRecord:int}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RecordItemExtDTO>> GetById(int IdRecord)
        {
            var recordDTO = await _recordService.GetByIdService(IdRecord);
            if (recordDTO == null)
            {
                return NotFound($"The record with ID {IdRecord} was not found");
            }

            var recordItemExtDTO = _mapper.Map<RecordItemExtDTO>(recordDTO);
            return Ok(recordItemExtDTO);
        }


        /// <summary>
        /// Get records sorted by title
        /// </summary>
        /// <param name="ascen">Ascending (true) or descending (false)</param>
        /// <returns>A list of records sorted by title</returns>
        [HttpGet("sortedByTitle/{ascen}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RecordItemExtDTO>>> GetSortedByTitle(bool ascen)
        {
            var records = await _recordService.GetSortedByTitleRecordService(ascen);
            var recordsItemExt = _mapper.Map<IEnumerable<RecordItemExtDTO>>(records);

            return Ok(recordsItemExt);
        }


        /// <summary>
        /// Search records by title
        /// </summary>
        /// <param name="text">Text to search in the title of the records</param>
        /// <returns>A list of records that match the search text</returns>
        [HttpGet("SearchByTitle/{text}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<RecordItemExtDTO>>> SearchByTitle(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return BadRequest("The search text cannot be empty");
            }

            var records = await _recordService.SearchByTitleRecordService(text);

            if (records == null || !records.Any())
            {
                return NotFound($"No disks were found that match the text '{text}'");
            }

            var recordsItemExt = _mapper.Map<IEnumerable<RecordItemExtDTO>>(records);
            return Ok(recordsItemExt);
        }


        /// <summary>
        /// Get records within a price range
        /// </summary>
        /// <param name="min">Minimum price</param>
        /// <param name="max">Maximum price</param>
        /// <returns>A list of records within the specified price range</returns>
        [HttpGet("byPriceRange")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<RecordItemExtDTO>>> GetByPriceRange(
            [FromQuery] decimal min, 
            [FromQuery] decimal max)
        {
            if (min < 0 || max < 0)
            {
                return BadRequest("The prices cannot be negative");
            }

            if (min > max)
            {
                return BadRequest("The minimum price cannot be greater than the maximum price");
            }

            var records = await _recordService.GetByPriceRangeRecordService(min, max);
            var recordsItemExt = _mapper.Map<IEnumerable<RecordItemExtDTO>>(records);

            return Ok(recordsItemExt);
        }


        /// <summary>
        /// Add a new record
        /// </summary>
        /// <param name="recordInsertDTO">Record data to add</param>
        /// <returns>The newly created record</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RecordItemExtDTO>> Add(RecordInsertDTO recordInsertDTO)
        {
            var validationResult = await _recordInsertValidator.ValidateAsync(recordInsertDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var recordDTO = await _recordService.AddService(recordInsertDTO);
            var recordItemExtDTO = _mapper.Map<RecordItemExtDTO>(recordDTO);

            return CreatedAtAction(nameof(GetById), new { IdRecord = recordItemExtDTO.IdRecord }, recordItemExtDTO);
        }


        /// <summary>
        /// Update an existing record
        /// </summary>
        /// <param name="id">ID of the record to update</param>
        /// <param name="recordUpdateDTO">Updated record data</param>
        /// <returns>The updated record</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RecordItemExtDTO>> Update(int id, RecordUpdateDTO recordUpdateDTO)
        {
            if (id != recordUpdateDTO.IdRecord)
            {
                return BadRequest("The ID of the route does not match the ID of the record");
            }

            var validationResult = await _recordUpdateValidator.ValidateAsync(recordUpdateDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var recordDTO = await _recordService.UpdateService(id, recordUpdateDTO);
            if (recordDTO == null)
            {
                return NotFound($"The disk with ID {id} was not found.");
            }

            var recordItemExtDTO = _mapper.Map<RecordItemExtDTO>(recordDTO);
            return Ok(recordItemExtDTO);
        }


        /// <summary>
        /// Update the stock of a record
        /// </summary>
        /// <param name="id">ID of the record to update</param>
        /// <param name="amount">Amount to add (positive) or subtract (negative) from the current stock</param>
        /// <returns>The updated stock of the record</returns>
        [HttpPut("{id:int}/updateStock/{amount:int}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStock(int id, int amount)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "The ID of the record is not valid" });
            }

            var record = await _recordService.GetByIdService(id);
            if (record == null)
            {
                return NotFound(new { message = $"The disk with ID {id} was not found." });
            }

            try
            {
                await _recordService.UpdateStockRecordService(id, amount);
                var updatedRecord = await _recordService.GetByIdService(id);
                
                if (updatedRecord == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, 
                        new { message = "The updated record was not found." });
                }

                return Ok(new
                {
                    message = $"The stock of the record with ID {id} has been updated in {amount} units",
                    nuevoStock = updatedRecord.Stock,
                    titulo = updatedRecord.TitleRecord,
                    stockAnterior = record.Stock
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { 
                    message = ex.Message,
                    idDisco = id,
                    cantidadSolicitada = amount,
                    stockActual = record.Stock
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { 
                    message = "An unexpected error occurred while updating the stock",
                    detalle = ex.Message
                });
            }
        }


        /// <summary>
        /// Delete a record by its ID
        /// </summary>
        /// <param name="IdRecord">ID of the record to delete</param>
        /// <returns>The deleted record or a 404 error if not found</returns>
        [HttpDelete("{IdRecord:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RecordItemExtDTO>> Delete(int IdRecord)
        {
            var recordDTO = await _recordService.DeleteService(IdRecord);
            if (recordDTO == null)
            {
                return NotFound($"The disk with ID {IdRecord} was not found");
            }

            var recordItemExtDTO = _mapper.Map<RecordItemExtDTO>(recordDTO);
            return Ok(recordItemExtDTO);
        }

    }
}
