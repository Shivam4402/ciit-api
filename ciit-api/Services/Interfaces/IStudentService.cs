using ciit_api.DTOs.Student;

namespace ciit_api.Services.Interfaces
{
    public interface IStudentService
    {
        Task<List<StudentBatchDetailsDto>> GetStudentWiseBatchDetails(int studentId);
        Task<List<StudentAttendanceDto>> GetStudentBatchWiseAttendance(int batchId, int registrationId);

    }
}
