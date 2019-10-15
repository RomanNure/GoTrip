using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

using GoNTrip.ServerAccess;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class ChangeAvatarQueryFactory : QueryFactory
    {
        private const string UPLOAD_AVATAR_SERVER_METHOD_NAME = "upload_avatar.php";

        private const string USER_ID_FIELD_NAME = "userId";
        private const string AVATAR_FIELD_NAME = "newAvatar";

        public async Task<IQuery> ChangeAvatar(long id, Stream avatar)
        {
            MultipartDataItem userId = null;
            MultipartDataItem av = null;

            await Task.Run(() =>
            {
                userId = new MultipartDataItem(new StringContent(id.ToString()), USER_ID_FIELD_NAME);
                av = new MultipartDataItem(new StreamContent(avatar), AVATAR_FIELD_NAME, id.ToString() + ".png");
            });

            return new Query(QueryMethod.POST_MULTIPART, UPLOAD_AVATAR_SERVER_METHOD_NAME, multipartData: new List<MultipartDataItem>() { userId, av });
        }
    }
}
