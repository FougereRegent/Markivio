using Markivio.Presentation.Middleware;

namespace Markivio.Presentation.GraphQl.User.UdpateUser;

using Descriptor = IObjectTypeDescriptor<Mutation>;

public static class UserMutation {
	extension(Descriptor descriptor) {
		public Descriptor MapUpdateUser() {
			descriptor
			  .Field(f => f.UpdateMyUser(default!, default!, default!))
			  .UseTransactionMiddleware()
			  .Type<UserInformationType>();
			return descriptor;
		}	
	}
}
