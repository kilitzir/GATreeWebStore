json <-
  '{ "names":[
  {"Name" : "Mario", "Age" : 32, "Occupation" : "Plumber"}, 
  {"Name" : "Peach", "Age" : 21, "Occupation" : "Princess"},
  {"Name" : "Bowser", "Occupation" : "Koopa"}
]}'
mydf <- fromJSON(json)
class(mydf)
cat(mydf)

readline(prompt = "heello")
