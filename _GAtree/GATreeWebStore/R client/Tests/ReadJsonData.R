library('httr')
library('jsonlite')
library("stringr")
# https://arxiv.org/abs/1403.2805
#https://cran.r-project.org/web/packages/httr/vignettes/quickstart.html

xx <-10
read11<-function() {
  url1 <- "http://localhost:29999/api/cycle"
  datarunID <- 15
  
# read data
query = list(datarunID = 17, 
             action = "GETATTRIBUTESTEXT"
)
ListJSON = toJSON(query,pretty = TRUE,auto_unbox = TRUE)
r <- POST(url = url1, 
          body = ListJSON,
          content_type_json()
          
)

cat(content(r, "text", encoding = "UTF-8"))
s1 <- content(r, "text")
mydf1 <- fromJSON(s1)
}
# get a job #1
for ( x in 1:10) {
  query = list(datarunID = datarunID, 
               action = "CLIENTGETJOB",
               TreeRecordId = x,
               result = "dfd"
  )
  ListJSON = toJSON(query,pretty = TRUE,auto_unbox = TRUE)
  #ListJSON
  
  r <- POST(url = url1, 
            body = ListJSON,
            content_type_json(),
            verbose(data_out = TRUE, data_in = TRUE, info = TRUE)
  )
}
s1 = content(r, "text")
cat(s1)
s2 = str_replace_all(s1,'\\\"','"')
cat(s2)
mydf1 <- fromJSON(s1)
mydf1$message
mydf2 <- fromJSON(mydf1$message)
mydf2
class(mydf2)
# the data is in dataframe 
# 
