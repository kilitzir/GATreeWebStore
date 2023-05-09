
#----------------------------------- print debug info
rrprint<- function(s)
{
  cat(s)  
  print(" ")
} 
#---------------------------------- merge two lists, replacing the items in listg
GA.mergeLists <- function(listg, list1) {
  merged_list <- listg
  for (item1 in names(list1)) {
    merged_list[[item1]] <- list1[[item1]]
  }
  return(merged_list)
}
#---------------------------------- 
