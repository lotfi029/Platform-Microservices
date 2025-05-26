using Grpc.Core;

namespace PlatformService.SyncDataServices.Grpc;

public class GrpcPlatformService(IPlatformRepo _platformRepo) : GrpcPlatform.GrpcPlatformBase
{
    public override async Task<GrpcPlatformResponse> GetAllPlatform(GetAllRequest reqeust, ServerCallContext  context)
    {
        var response = new GrpcPlatformResponse();
        var platforms = await _platformRepo.GetAllPlatfromAsync();

        foreach(var plat in platforms)
        {
            response.Platforms.Add(plat.Adapt<GrpcPlatformModel>());
        }

        return response;
    }
}
