// Create users
CREATE 
(:User {id:'abfc1edc-8b2e-4351-8e55-a0daf0d60d46',userDisplayName:'SuperMario',userName:'supermario'})
-[:DESCRIBED_BY]->
(:UserProfile {id:'abfc1edc-8b2e-4351-8e55-a0daf0d60d46',description:'Hello I am SuperMario!',pictureUrl:'https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg'}), 
(:User {id:'2ea3a292-f6e4-40d7-98c0-fa721cfae443',userDisplayName:'Jeff',userName:'jeffery'})
-[:DESCRIBED_BY]->
(:UserProfile {id:'2ea3a292-f6e4-40d7-98c0-fa721cfae443',description:'Hello I am Jeff!',pictureUrl:'https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg'}), 
(:User {id:'f89c35de-fb43-4fb4-848d-68c8b625ca38',userDisplayName:'Tobin',userName:'kick'})
-[:DESCRIBED_BY]->
(:UserProfile {id:'f89c35de-fb43-4fb4-848d-68c8b625ca38',description:'Hello I am TOBIN!',pictureUrl:'https://icon-library.net/images/default-user-icon/default-user-icon-8.jpg'});

// Follows
MATCH (a:User {id: 'abfc1edc-8b2e-4351-8e55-a0daf0d60d46' }), (b:User { id: '2ea3a292-f6e4-40d7-98c0-fa721cfae443' }) CREATE (a)<-[:FOLLOWED_BY {followedDateTime: '2021-04-25 21:45:40Z'}]-(b);
MATCH (a:User {id: 'abfc1edc-8b2e-4351-8e55-a0daf0d60d46' }), (b:User { id: 'f89c35de-fb43-4fb4-848d-68c8b625ca38' }) CREATE (a)<-[:FOLLOWED_BY {followedDateTime: '2021-04-25 21:45:40Z'}]-(b);
MATCH (a:User {id: '2ea3a292-f6e4-40d7-98c0-fa721cfae443' }), (b:User { id: 'abfc1edc-8b2e-4351-8e55-a0daf0d60d46' }) CREATE (a)<-[:FOLLOWED_BY {followedDateTime: '2021-04-25 21:45:40Z'}]-(b);

// Kweets
MATCH (a:User {id: 'abfc1edc-8b2e-4351-8e55-a0daf0d60d46' }) CREATE (a)<-[:KWEETED_BY]-(:Kweet {id:'b3a84788-dda1-495a-a30f-0e7e35e04c37', message:'Hello Kwetter1!',createdDateTime:'2021-04-25 21:45:40Z'});
MATCH (a:User {id: 'abfc1edc-8b2e-4351-8e55-a0daf0d60d46' }) CREATE (a)<-[:KWEETED_BY]-(:Kweet {id:'cf711301-6430-4a2b-addd-8ab547e51127', message:'Hello Kwetter2!',createdDateTime:'2021-04-25 21:46:40Z'});
MATCH (a:User {id: 'abfc1edc-8b2e-4351-8e55-a0daf0d60d46' }) CREATE (a)<-[:KWEETED_BY]-(:Kweet {id:'cc711301-6430-4a2b-addd-8ab547e51127', message:'Kwetterwette rHelloKwette rHelloKwetter HelloKwetterHe lloKwetterHel loKwetterHelloKwe tterHelloKwetter HelloKwetter Hello!!',createdDateTime:'2021-04-25 21:46:40Z'});
MATCH (a:User {id: '2ea3a292-f6e4-40d7-98c0-fa721cfae443' }) CREATE (a)<-[:KWEETED_BY]-(:Kweet {id:'bb76d10a-62d3-4d0e-872f-bb9ca65b35c1', message:'Hello Kwetter3!',createdDateTime:'2021-04-25 21:48:40Z'});
MATCH (a:User {id: '2ea3a292-f6e4-40d7-98c0-fa721cfae443' }) CREATE (a)<-[:KWEETED_BY]-(:Kweet {id:'24b03599-efeb-4469-b7c1-b1c497f809ad', message:'Hello Kwetter4!',createdDateTime:'2021-04-25 21:49:40Z'});
MATCH (a:User {id: 'f89c35de-fb43-4fb4-848d-68c8b625ca38' }) CREATE (a)<-[:KWEETED_BY]-(:Kweet {id:'77c789fd-64f4-4ad4-99ba-cc6946d0e71e', message:'Hello Kwetter5!',createdDateTime:'2021-04-25 21:58:40Z'});
MATCH (a:User {id: 'f89c35de-fb43-4fb4-848d-68c8b625ca38' }) CREATE (a)<-[:KWEETED_BY]-(:Kweet {id:'a3153784-4cff-43db-938d-3ffbbc4beb25', message:'Hello Kwetter6!',createdDateTime:'2021-04-25 21:59:40Z'});

// Kweet likes
MATCH (a:User {id: 'abfc1edc-8b2e-4351-8e55-a0daf0d60d46' }), (b:Kweet { id: 'b3a84788-dda1-495a-a30f-0e7e35e04c37' }) CREATE (a)<-[:LIKED_BY {id: '9e4d277d-cdc5-4b4a-82a3-9c3cf6e6e32f', likedDateTime: '2021-04-25 21:47:40Z'}]-(b);
MATCH (a:User {id: '2ea3a292-f6e4-40d7-98c0-fa721cfae443' }), (b:Kweet { id: 'b3a84788-dda1-495a-a30f-0e7e35e04c37' }) CREATE (a)<-[:LIKED_BY {id: '1c17cca2-9ef7-460d-9441-f6bb87079da1', likedDateTime: '2021-04-25 21:47:40Z'}]-(b);
MATCH (a:User {id: 'f89c35de-fb43-4fb4-848d-68c8b625ca38' }), (b:Kweet { id: 'b3a84788-dda1-495a-a30f-0e7e35e04c37' }) CREATE (a)<-[:LIKED_BY {id: 'df9715c5-2364-49d1-a980-ec84ab87d963', likedDateTime: '2021-04-25 21:48:40Z'}]-(b);
