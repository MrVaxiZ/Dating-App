export interface Group {
    name: string;
    connections: Connection[];
}

export interface Connection {
    connetionId: string;
    username: string;
}