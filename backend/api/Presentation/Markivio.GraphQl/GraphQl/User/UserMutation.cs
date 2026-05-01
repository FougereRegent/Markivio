using Markivio.Presentation.GraphQl.User.UdpateUser;
namespace Markivio.Presentation.GraphQl.User;

using Descriptor = IObjectTypeDescriptor<Mutation>;

public static class UserMutation {
	extension(Descriptor descriptor) {
		public Descriptor MapUserMutation() {
			descriptor.MapUpdateUser();
			return descriptor;
		}	
	}
}
