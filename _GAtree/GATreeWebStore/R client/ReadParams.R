library(httr)
library(jsonlite)
library(stringr)

source("Utils.R")


# implementing roxygen2 for documentation. For more info :
# https://cran.r-project.org/web/packages/roxygen2/


#------------ SET GLOBAL VARIABLES

#' sets the Global variables
#' 
#' @return none
#' @examples
#' setGlobalVariables()

GA.setGlobalVariables <- function()
{
  print("GA.setGlobalVariables begin ----------")
  rm(list = ls()) # to clean the environment
  gc()
  set.seed(280870) # for runif
  assign("globalvariables1",list(), envir = .GlobalEnv)
  #set the request strings
  list_data <- list("REGISTERCLIENT","UNREGISTERCLIENT","GETPARAMETERS","GETDATA",
                    "CLIENTGETJOB","CROSSOVER","MUTATION","CREATETREES","GETATTRIBUTESTEXT","GETATTRIBUTES","BUSY")
  names(list_data) <- c("REGISTERCLIENT","UNREGISTERCLIENT","GETPARAMETERS","GETDATA",
                        "CLIENTGETJOB","CROSSOVER","MUTATION","CREATETREES","GETATTRIBUTESTEXT","GETATTRIBUTES","BUSY")
  assign("actions",list_data, envir = .GlobalEnv)
  #set the response error
  list_data <- list("NONE", "OK", "ERROR")
  names(list_data) <- c("NONE", "OK", "ERROR")
  assign("server_responds",list_data, envir = .GlobalEnv)
  globalvariables1$tree_labels <<- c("id","var","split","class","lChild","rChild") 
  
  assign("server_responds",list_data, envir = .GlobalEnv)
  print("GA.setGlobalVariables end ----------")
  
}
#--------------------------- READ DATA SET
#' Reads the data set using web api
#' 
#' @param Url The URL of the web api.
#' @param datarunID The Id for the test run.
#' @return The data in  data frame format.
#' @examples
#' readData("http://localhost:28888/api/cycle", 15)
#' 
GA.readDataSet <- function(Url11,datarunID) { 
  
  query = list(datarunID = datarunID, 
               action = "GETDATA"
  )
  rrprint("readDataSet begin -----------------:")
  
  ListJSON <- toJSON(query,pretty = TRUE,auto_unbox = TRUE)
  rrprint(paste("\t Json to send: ",ListJSON,"\n"))
  r <- POST(url = Url11, 
            body = ListJSON,
            content_type_json()
  )
  s1 <- content(r, "text")
  rrprint(paste("\t Json to receive: ",s1))
  mydf1 <- fromJSON(s1)
  mydf2 <- fromJSON(mydf1$message)
  # Add a new column with integer values corresponding to the factor levels
  # This should be a function but anyways
  factor_col <- factor(mydf2$class, levels = unique(mydf2$class))
  mydf2$classLevel <- as.integer(factor_col)
  
  mydf2$classLevel <- as.integer(factor_col)
  rrprint("readDataSet end -----------------:")
  return (list(dataset = mydf2))
}
# -------------------------------- READ RUN PARAMETERS
#' Reads the parameters using web api
#' 
#' @param Url The URL of the web api.
#' @param datarunID The Id for the test run.
#' @return The data in  data frame format.
#' @examples
#' readParameters("http://ddd.com, 14)

GA.readRunParameters <- function(Url11,datarunID) 
{ 
  print("readRunParameters begin ---------")
  query = list(datarunID = datarunID, 
               action = "GETPARAMETERS"
  )
  ListJSON <- toJSON(query,pretty = TRUE,auto_unbox = TRUE)
  rrprint(paste("\t Json to send: ",ListJSON))
  r <- POST(url = Url11, 
            body = ListJSON,
            content_type_json()
  )
  #cat(content(r, "text", encoding = "UTF-8"))
  s1 = content(r, "text")
  mydf1 <- fromJSON(s1)
  rrprint(paste("\t Json to receive: ",mydf1))
  mydf2 <- fromJSON(mydf1$message)
  rrprint(paste("\t Json parameters: ",mydf1))
  mydf3 <- ""
  tryCatch(
    {
      mydf3 <- fromJSON(mydf2$otherParams)
      rrprint(paste("\t Json extra parameters: ",mydf1))
    },
    error = function(e) {
      message('\t readRunParameters: An Error Occurred in reading optional parameters. Not exist?')
      print(e)
    },
    warning = function(w) {
      message('\t readRunParameters: A Warning Occurred in reading optional parameters :')
      print(w)
    }
  )
  rrprint("\nreadRunParameters end ---------")
  return(list(parameters=mydf2, extraParameters = mydf3))
}
# -------------------------------- READ ATTRIBUTES TEXT
#' Reads the parameters using web api
#' 
#' @param Url The URL of the web api.
#' @param datarunID The Id for the test run.
#' @return The data in  data frame format.
#' @examples
#' readParameters("http://ddd.com, 14)

GA.readDataAttributes <- function(Url11,datarunID) 
{ 
  query = list(datarunID = datarunID, 
               action = actions$GETATTRIBUTESTEXT
  )
  rrprint("readDataAttributes: begin --------------")
  ListJSON <- toJSON(query,pretty = TRUE,auto_unbox = TRUE)
  rrprint(paste("\t","Json to send:", ListJSON))
  r <- POST(url = Url11, 
            body = ListJSON,
            content_type_json()
  )
  #cat(content(r, "text", encoding = "UTF-8"))
  rrprint(paste("\t","Json to recieve:", r))
  s1 = content(r, "text")
  mydf1 <- fromJSON(s1)
  browser()
  rrprint(paste("\t","message to receive:", mydf1))
  rrprint("readDataAttributes: end --------------")
  return(list(dataAttributesText = mydf1$message))
}
# ------------------------READ DATA ATTRIBUTES TABLE
GA.readDataAttributesTable <- function(Url11,datarunID) 
{ 
  rrprint("GA.readDataAttributesTable : begin -------------------")
  query = list(datarunID = datarunID, 
               action = actions$GETATTRIBUTES
  )
  ListJSON <- toJSON(query,pretty = TRUE,auto_unbox = TRUE)
  rrprint(paste("\tjson to send:",ListJSON))
  r <- POST(url = Url11, 
            body = ListJSON,
            content_type_json()
  )
  s1 = content(r, "text")
  rrprint(paste("\tJson to receive:",s1))
  mydf1 <- fromJSON(s1)
  mydf2 <- fromJSON(mydf1$message)
  mydf2 <- mydf2[,-1] # first line has the index, we don't need it.
  #mydf2 <- data.frame(str_split_fixed(mydf2, "\\s+", n = 2))
  browser()
  colnames(mydf2) <- c("name","type","districtValues","isClass","hasNullValues","maxValue","minValue")
  rrprint("GA.readDataAttributesTable : end -------------------")
  return(list(dataAttributesTable = mydf2))
}
#----------------------- REGISTER THICLIENT GUID
#' Register the client using web api
#' 
#' @param Url The URL of the web api.
#' @param datarunID The Id for the test run.
#' @return The Guid in string format
#' @examples
#' registerThinClient("http://ddd.com, 14)

GA.registerThinClient <- function(Url,datarunID) 
{ 
  rrprint("RegisterThinclient begin ------------------")
  query = list(datarunID = datarunID,
               action = "REGISTERCLIENT"
  )
  ListJSON = toJSON(query,pretty = TRUE,auto_unbox = TRUE)
  rrprint(paste("\t Json to send:",ListJSON))
  r <- POST(url = Url, 
            body = ListJSON,
            content_type_json()
  )
  s1 = content(r, "text")
  mydf1 <- fromJSON(s1)
  rrprint(paste("\t Json to receive:",s1))
  mydf2 <- fromJSON(mydf1$message)
  rrprint("RegisterThinclient end ------------------")
  return(list(Guid = mydf2[2]))
}
#----------------------- UNREGISTER THIΝCLIENT
#' UNRegister the client using web api
#' 
#' @param Url The URL of the web api.
#' @param datarunID The Id for the test run.
#' @param guid The Guid of the client
#' @return The Guid in string format
#' @examples
#' registerThinClient("http://ddd.com, 14)

unRegisterThinClient <- function(Url, datarunID, guid) 
{ 
  query = list(datarunID = datarunID, 
               action = "UNREGISTERCLIENT",
               datarunID = datarunID,
               clientGuid = guid
  )
  ListJSON = toJSON(query,pretty = TRUE,auto_unbox = TRUE)
  r <- POST(url = Url, 
            body = ListJSON,
            content_type_json()
  )
  cat(content(r, "text", encoding = "UTF-8"))
  s1 = content(r, "text")
  mydf1 <- fromJSON(s1)
  mydf1$message
  mydf2 <- fromJSON(mydf1$message)
}
#----------------------- READ USER PARAMETERS
#' Reads user parameters
#' 
#' @return The parameters in a list
#' @examples
#' readUserParams()

GA.readUserParameters <- function() 
{ 
  print("readUserParameters begin -----------")
  url1<-readline(prompt = "Please provide URL:(http://localhost:28888/api/cycle) ") # TODO check the parameters!!
  datarun1 <- readline(prompt = "Please provide DataRunId: ")
  x <- list(url = url1,datarun = datarun1)
  print("readUserParameters end -----------")
  return (x)
}
#----------------------- MAIN CIRCLE ---- MAIN -- 
GA.readAllParams <- function()
{
  print("readAllParams begin -------")
  GA.setGlobalVariables();
  cc <- GA.readUserParameters(); 
  globalvariables1 <<- GA.mergeLists(globalvariables1,cc)
  cc <- GA.readRunParameters (globalvariables1$url,globalvariables1$datarun)
  globalvariables1 <<- GA.mergeLists(globalvariables1,cc)
  cc <- GA.readDataSet(Url = globalvariables1$url,globalvariables1$datarun)
  globalvariables1 <<- GA.mergeLists(globalvariables1,cc)
  cc <- GA.readDataAttributes(Url = globalvariables1$url,globalvariables1$datarun)
  globalvariables1 <<- GA.mergeLists(globalvariables1,cc)
  cc <- GA.readDataAttributesTable(Url = globalvariables1$url,globalvariables1$datarun)
  globalvariables1 <<- GA.mergeLists(globalvariables1,cc)
  cc <- GA.registerThinClient(Url = globalvariables1$url,globalvariables1$datarun)
  globalvariables1 <<- GA.mergeLists(globalvariables1,cc)
  cc <- GA.createAttributes()
  globalvariables1 <<- GA.mergeLists(globalvariables1,cc)
  print("readAllParams end -------")
}
