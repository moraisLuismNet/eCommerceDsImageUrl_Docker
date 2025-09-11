using AutoMapper;
using eCommerceDs.DTOs;
using eCommerceDs.Models;
using eCommerceDs.Repository;

namespace eCommerceDs.Services
{
    public class RecordService : IRecordService
    {
        private readonly IRecordRepository<Record> _recordRepository;
        private readonly IMapper _mapper;
        public List<string> Errors { get; }
        public RecordService(IRecordRepository<Record> recordRepository,
            IMapper mapper)
        {
            _recordRepository = recordRepository;
            _mapper = mapper;
            Errors = new List<string>();
        }


        public async Task<IEnumerable<RecordDTO>> GetService()
        {
            var records = await _recordRepository.GetRecordRepository();

            return records.Select(record => _mapper.Map<RecordDTO>(record));
        }


        public async Task<RecordDTO> GetByIdService(int id)
        {
            var record = await _recordRepository.GetByIdRepository(id);

            if (record != null)
            {
                var recordDTO = _mapper.Map<RecordDTO>(record);
                return recordDTO;
            }

            return null;
        }


        public async Task<Record> GetRecordByIdRecordService(int id)
        {
            return await _recordRepository.GetByIdAsyncRecordRepository(id);
        }

        public async Task<IEnumerable<RecordDTO>> GetSortedByTitleRecordService(bool ascending)
        {
            var records = await _recordRepository.GetSortedByTitleRecordRepository(ascending);

            return records.Select(record => _mapper.Map<RecordDTO>(record));
        }


        public async Task<IEnumerable<RecordDTO>> SearchByTitleRecordService(string text)
        {
            var records = await _recordRepository.SearchByTitleRecordRepository(text);

            return records.Select(record => _mapper.Map<RecordDTO>(record));
        }


        public async Task<IEnumerable<RecordDTO>> GetByPriceRangeRecordService(decimal min, decimal max)
        {
            var records = await _recordRepository.GetByPriceRangeRecordRepository(min, max);

            return records.Select(record => _mapper.Map<RecordDTO>(record));
        }


        public async Task<RecordDTO> AddService(RecordInsertDTO recordInsertDTO)
        {
            if (!await _recordRepository.GroupExistsRecordRepository(recordInsertDTO.GroupId))
            {
                throw new ArgumentException($"The with ID {recordInsertDTO.GroupId} does not exist");
            }

            var record = _mapper.Map<Record>(recordInsertDTO);
            record.ImageRecord = recordInsertDTO.ImageUrl;

            await _recordRepository.AddRepository(record);
            await _recordRepository.SaveRepository();

            return _mapper.Map<RecordDTO>(record);
        }


        public async Task<RecordDTO> UpdateService(int id, RecordUpdateDTO recordUpdateDTO)
        {
            var record = await _recordRepository.GetByIdRepository(id);
            if (record is null) return null;

            _mapper.Map(recordUpdateDTO, record);

            // Map ImageUrl to ImageRecord
            if (!string.IsNullOrEmpty(recordUpdateDTO.ImageUrl))
            {
                record.ImageRecord = recordUpdateDTO.ImageUrl;
            }

            _recordRepository.UpdateRepository(record);
            await _recordRepository.SaveRepository();

            return _mapper.Map<RecordDTO>(record);
        }


        public async Task UpdateStockRecordService(int id, int amount)
        {
            var record = await _recordRepository.GetByIdRepository(id);

            if (record == null)
            {
                throw new InvalidOperationException($"Record with ID {id} not found");
            }

            if (amount < 0 && Math.Abs(amount) > record.Stock)
            {
                throw new InvalidOperationException("The decrease cannot be greater than the available stock");
            }

            record.Stock += amount;
            await _recordRepository.UpdateStockRecordRepository(record);
        }


        public async Task<RecordDTO> DeleteService(int id)
        {
            var record = await _recordRepository.GetByIdRepository(id);

            if (record != null)
            {
                var recordDTO = _mapper.Map<RecordDTO>(record);

                // We did not remove the image from Imgur because it is an external service.
                // If at some point we need to delete it, we would need the Imgur API.

                _recordRepository.DeleteRepository(record);
                await _recordRepository.SaveRepository();

                return recordDTO;
            }

            return null;
        }

        public async Task UpdateRecordRecordService(Record record)
        {
            await _recordRepository.UpdateAsyncRecordRepository(record);
        }

    }
}
