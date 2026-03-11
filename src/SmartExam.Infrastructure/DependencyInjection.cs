using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartExam.Application.Interfaces;
using SmartExam.Infrastructure.Persistence;
using SmartExam.Infrastructure.Services;

namespace SmartExam.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IScienceService, ScienceService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<ITopicService, TopicService>();
        services.AddScoped<ILanguageService, LanguageService>();
        services.AddScoped<IZoneService, ZoneService>();
        services.AddScoped<ITeacherService, TeacherService>();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IAttachmentService, AttachmentService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IFileStorageService, LocalFileStorageService>();

        return services;
    }
}
