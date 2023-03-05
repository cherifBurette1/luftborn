namespace luftborn.Ground
{
    public enum ValidationStatus
    {
        BadRequest = 400,
        NotFound = 404,
        Unauthorized = 401,
        Accepted = 202
    }
    public enum AuditActionTypes
    {
        Unknown = 0,
        Create = 1,
        Update = 2,
        Delete = 3
    }
}