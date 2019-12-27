using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

using GoNTrip.ServerAccess;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class ChangeAvatarQueryFactory : QueryFactory
    {
        private const string UPLOAD_AVATAR_SERVER_METHOD_NAME = "upload_user_avatar.php";

        private const string AVATAR_FIELD_NAME = "user_avatar";

        public async Task<IQuery> ChangeAvatar(long id, Stream avatar)
        {
            MultipartDataItem av = null;

            await Task.Run(() =>
            {
                av = new MultipartDataItem(new StreamContent(avatar), AVATAR_FIELD_NAME, id.ToString() + ".png");
            });

            return new Query(QueryMethod.POST_MULTIPART, UPLOAD_AVATAR_SERVER_METHOD_NAME, 
                             multipartData: new List<MultipartDataItem>() { av });
        }
    }
}
