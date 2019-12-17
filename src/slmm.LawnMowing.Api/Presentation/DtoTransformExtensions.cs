namespace slmm.LawnMowing.Api.Presentation
{
    using Dtos;
    using Model;

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
