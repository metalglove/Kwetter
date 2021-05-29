//Timeline
CALL {
    MATCH (:User {id: 'abfc1edc-8b2e-4351-8e55-a0daf0d60d46'})<-[:FOLLOWED_BY]-(user:User)<-[:KWEETED_BY]-(kweet:Kweet)
    RETURN user, kweet
    UNION
    MATCH (user:User {id: 'abfc1edc-8b2e-4351-8e55-a0daf0d60d46'})<-[:KWEETED_BY]-(kweet:Kweet)
    RETURN user, kweet
    UNION
    MATCH (:User {id: 'abfc1edc-8b2e-4351-8e55-a0daf0d60d46'})-[:MENTIONED_IN]->(kweet:Kweet)-[:KWEETED_BY]->(user:User)
    RETURN user, kweet
}
WITH user, kweet
ORDER BY kweet.createdDateTime DESC
SKIP 0
LIMIT 25
CALL {
    WITH kweet
    OPTIONAL MATCH (liker:User)<-[:LIKED_BY]-(kweet)
    RETURN liker
}
CALL {
    WITH kweet
    OPTIONAL MATCH (i:User {id: 'abfc1edc-8b2e-4351-8e55-a0daf0d60d46'})<-[:LIKED_BY]-(kweet)
    RETURN (i.id IS NOT NULL) as liked
}
RETURN user.id, user.name, user.displayName, user.profilePictureUrl, kweet.createdDateTime, kweet.id, kweet.message, count(liker) as likes, liked