library('httr')
library('jsonlite')
library("stringr")
# https://arxiv.org/abs/1403.2805
#https://cran.r-project.org/web/packages/httr/vignettes/quickstart.html

url1 <- "http://localhost:29999/api/cycle"
datarunID <- 15
xx <-10
# read data
query = list(datarunID = datarunID, 
             action = "GETPARAMETERS"
)
ListJSON = toJSON(query,pretty = TRUE,auto_unbox = TRUE)
r <- POST(url = url1, 
          body = ListJSON,
          content_type_json()
          
)
cat(content(r, "text", encoding = "UTF-8"))

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