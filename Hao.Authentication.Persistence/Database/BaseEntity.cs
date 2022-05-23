namespace Hao.Authentication.Persistence.Database
{
    public class BaseEntity
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 最近修改
        /// </summary>
        public DateTime? LastModifiedAt { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeletedAt { get; set; }
    }
}
