library(httr)
library(jsonlite)
# https://cran.r-project.org/web/packages/httr/vignettes/quickstart.html
# https://www.tutorialspoint.com/how-to-convert-a-list-to-json-in-r
#---------------------
query = list(datarunID = "4", 
             action = "CLIENTGETJOB",
             TreeRecordId = "73",
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
status_code(r)
http_status(r)
content(r)$args
content(r, "text")
stringi::stri_enc_detect(content(r, "raw"))
str(content(r, "parsed"))
headers(r)
cookies(r)
