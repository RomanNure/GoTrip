using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

using GoNTrip.ServerAccess;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class ChangeAvatarQueryFactory : QueryFactory
    {
        private const string UPLOAD_AVATAR_SERVER_METHOD_NAME = "upload/fileupload";

        private const string USER_ID_FIELD_NAME = "userId";
        private const string AVATAR_FIELD_NAME = "newAvatar";
        private const string FILENAME_FIELD_NAME = "filename";

        public async Task<IQuery> ChangeAvatar(long id, Stream avatar)
        {
            MultipartDataItem userId = null;
            MultipartDataItem filename = null;
            MultipartDataItem av = null;

            await Task.Run(() =>
            {
                userId = new MultipartDataItem(new StringContent(id.ToString()), USER_ID_FIELD_NAME);
                filename = new MultipartDataItem(new StringContent(AVATAR_FIELD_NAME), FILENAME_FIELD_NAME);
                av = new MultipartDataItem(new StreamContent(avatar), AVATAR_FIELD_NAME, id.ToString() + ".png");
            });

            return new Query(QueryMethod.POST_MULTIPART, UPLOAD_AVATAR_SERVER_METHOD_NAME, multipartData: new List<MultipartDataItem>() { userId, filename, av });
        }
    }
}
