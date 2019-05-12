

namespace Slmm.Api.Presentation
{ 
    using Slmm.Api.Presentation.Dtos;
    using Slmm.Domain;

    public static class DtoTransformExtensions
    {
        public static PositionDto ToDto(this Position domanModel)
        {
            return new PositionDto
            {
                X = domanModel.Coordinates.X,
                Y = domanModel.Coordinates.Y,
                Orientation = domanModel.Orientation.ToString()
            };
        }
    }
}
