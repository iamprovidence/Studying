export const SERVER_URL = "https://task-manger-demo.azurewebsites.net";
export const SERVER_URL_API = SERVER_URL + "/api";

export const API = 
{    
    USER: 
    {
        SINGLE_FORMAT:  SERVER_URL_API + '/users/{0}',
        ALL:            SERVER_URL_API + "/users",
        UPDATE:         SERVER_URL_API + "/users",
        CREATE:         SERVER_URL_API + "/users",
        DELETE_FORMAT:  SERVER_URL_API + "/users/{0}",
    },
    TEAM:
    {
        SINGLE_FORMAT:  SERVER_URL_API + "/teams/{0}",
        ALL:            SERVER_URL_API + "/teams",
        UPDATE:         SERVER_URL_API + "/teams",
        CREATE:         SERVER_URL_API + "/teams",
        DELETE_FORMAT:  SERVER_URL_API + "/teams/{0}",
    },
    PROJECT:
    {
        SINGLE_FORMAT:  SERVER_URL_API + "/projects/{0}",
        ALL:            SERVER_URL_API + "/projects",
        UPDATE:         SERVER_URL_API + "/projects",
        CREATE:         SERVER_URL_API + "/projects",
        DELETE_FORMAT:  SERVER_URL_API + "/projects/{0}",
    },
    TASK:
    {
        SINGLE_FORMAT:  SERVER_URL_API + "/tasks/{0}",
        ALL:            SERVER_URL_API + "/tasks",
        UPDATE:         SERVER_URL_API + "/tasks",
        CREATE:         SERVER_URL_API + "/tasks",
        DELETE_FORMAT:  SERVER_URL_API + "/tasks/{0}",
    },
    TASK_STATE:
    {
        ALL:            SERVER_URL_API + "/tasks/states"
    }
};