//Timeline
CALL {
    MATCH (user:User {id: 'bb7f4b58-cddb-462f-a6ef-c735c44c8276'})-[:FOLLOWS]->(user:User)<-[:KWEETED_BY]-(kweet:Kweet)
    RETURN user, kweet
    UNION
    MATCH (user:User)<-[:KWEETED_BY]-(kweet:Kweet)
    RETURN user, kweet
}
WITH user, kweet
SKIP 0
LIMIT 25
CALL {
    WITH kweet
    OPTIONAL MATCH (liker:User)<-[:LIKED_BY]-(kweet)
    RETURN liker
}
CALL {
    WITH kweet
    OPTIONAL MATCH (i:User {id: 'bb7f4b58-cddb-462f-a6ef-c735c44c8276'})<-[:LIKED_BY]-(kweet)
    RETURN (i.id IS NOT NULL) as liked
}
RETURN user.id, user.userDisplayName, kweet.createdDateTime, kweet.id, kweet.message, count(liker) as likes, liked