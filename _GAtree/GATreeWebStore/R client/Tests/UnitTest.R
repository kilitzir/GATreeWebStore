library('httr')
library('jsonlite')
datarunID <- 8 
# RegisterClient
query = list(datarunID = datarunID, 
             action = "REGISTERCLIENT",
             TreeRecordId = 1119,
             result = "dfd"
)
ListJSON = toJSON(query,pretty = TRUE,auto_unbox = TRUE)
ListJSON
url1 <- "http://localhost:29999/api/cycle"
r <- POST(url = url1, 
          body = ListJSON,
          content_type_json(),
          verbose(data_out = TRUE, data_in = TRUE, info = TRUE)
)
datarunID <- 9
xx <-10
# get a job #1
for ( x in 1:10) {
            query = list(datarunID = datarunID, 
                         action = "CLIENTGETJOB",
                         TreeRecordId = x,
                         result = "dfd"
            )
            ListJSON = toJSON(query,pretty = TRUE,auto_unbox = TRUE)
            #ListJSON
            url1 <- "http://localhost:29999/api/cycle"
            r <- POST(url = url1, 
                      body = ListJSON,
                      content_type_json(),
                      verbose(data_out = TRUE, data_in = TRUE, info = TRUE)
            )
}
# Post the init trees
datarunID <- 9
xx<- 1201
for (x in xx:(xx+10)) {
  query = list(datarunID = datarunID, 
               action = "CREATETREES",
               TreeRecordId = x,
               result =  paste("{\"trees\":[{\"tree\":\"tree11\",\"evaluation\": \"",sample(1:100,1),"\"},{\"tree\": \"tree22\",\"evaluation\":\"",sample(1:100,1), "\"}] }")
              )
  ListJSON = toJSON(query,pretty = TRUE,auto_unbox = TRUE)
#  ListJSON
  url1 <- "http://localhost:29999/api/cycle"
  r <- POST(url = url1, 
            body = ListJSON,
            content_type_json(),
            verbose(data_out = TRUE, data_in = TRUE, info = TRUE)
      )
}



#----------- CrossOver - get jobs
id <- 1119
datarunID <-9
for (x in 1:6){
        query = list(datarunID = datarunID, 
                     action = "CLIENTGETJOB",
                     TreeRecordId = 1119,
                     result = "dfd"
        )
        ListJSON = toJSON(query,pretty = TRUE,auto_unbox = TRUE)
        ListJSON
        url1 <- "http://localhost:29999/api/cycle"
        r <- POST(url = url1, 
                  body = ListJSON,
                  content_type_json(),
                  verbose(data_out = TRUE, data_in = TRUE, info = TRUE)
        )
}

#------------- CrossOver send the results
# Post the init trees
datarunID <- 9
xx<- 1204
for (x in xx:(xx+10)) {
  query = list(datarunID = datarunID, 
               action = "CROSSOVER",
               TreeRecordId = x,
               result =  paste("{\"trees\":[{\"tree\":\"tree11\",\"evaluation\": \"",sample(1:100,1),"\"},{\"tree\": \"tree22\",\"evaluation\":\"",sample(1:100,1), "\"}] }")
  )
  ListJSON = toJSON(query,pretty = TRUE,auto_unbox = TRUE)
  #  ListJSON
  url1 <- "http://localhost:29999/api/cycle"
  r <- POST(url = url1, 
            body = ListJSON,
            content_type_json(),
            verbose(data_out = TRUE, data_in = TRUE, info = TRUE)
  )
}
#----------- Mutation - get jobs
id <- 1119
datarunID <-9
for (x in 1:6){
  query = list(datarunID = datarunID, 
               action = "CLIENTGETJOB",
               TreeRecordId = 1119,
               result = "dfd"
  )
  ListJSON = toJSON(query,pretty = TRUE,auto_unbox = TRUE)
  ListJSON
  url1 <- "http://localhost:29999/api/cycle"
  r <- POST(url = url1, 
            body = ListJSON,
            content_type_json(),
            verbose(data_out = TRUE, data_in = TRUE, info = TRUE)
  )
}

#------------- Mutation send the results
# Post the init trees
datarunID <- 9
xx<- 1214
for (x in xx:(xx+5)) {
  query = list(datarunID = datarunID, 
               action = "MUTATION",
               TreeRecordId = x,
               result =  paste("{\"tree\":\"tree11\",\"evaluation\":\"",sample(1:100,1),"\"}")
  )
  ListJSON = toJSON(query,pretty = TRUE,auto_unbox = TRUE)
  #  ListJSON
  url1 <- "http://localhost:29999/api/cycle"
  r <- POST(url = url1, 
            body = ListJSON,
            content_type_json(),
            verbose(data_out = TRUE, data_in = TRUE, info = TRUE)
  )
}


