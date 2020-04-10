namespace ApiFormat
{
    public interface IModel : IDTO
    {
        
    }

    //TODO: Create To<X>Model and To<X>DTO conversion functions
    /*
     * Example
     * public static GroupModel ToGroupModel(this IDTO source)
     * {
     *      return new GroupModel()
     *      {
     *        assign properties.
     *          I don't think navigational properties are needed.
     *      }
     * }
     */

    //TODO: Convert IDTO derived interfaces to classes and delete the implemented class (Fx: Convert IItemDTO to ItemDto and delete Item)
}