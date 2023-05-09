import os

def countlines(start, lines=0, header=False, begin_start=None):
    if header:
        print('{:>10} |{:>10} | {:<20}'.format('ADDED', 'TOTAL', 'FILE'))
        print('{:->11}|{:->11}|{:->20}'.format('', '', ''))

    for thing in os.listdir(start):
        thing = os.path.join(start, thing)
        if os.path.isfile(thing):
            if thing.endswith('.cs') or thing.endswith('.cshtml') :
                with open(thing, 'r') as f:
                    newlines = f.readlines()
                    newlines = len(newlines)
                    lines += newlines

                    if begin_start is not None:
                        reldir_of_thing = '.' + thing.replace(begin_start, '')
                    else:
                        reldir_of_thing = '.' + thing.replace(start, '')

                    print('{:>10} |{:>10} | {:<20}'.format(
                            newlines, lines, reldir_of_thing))


    for thing in os.listdir(start):
        thing = os.path.join(start, thing)
        if os.path.isdir(thing):
            lines = countlines(thing, lines, header=False, begin_start=start)
    
    return lines

x0 = r'E:\GitHub_test1\Test1\_GAtree\GATreeWebStore\GAtreeStore\GAtreeWebStore\\'
x1 = countlines(x0 +'Controllers');
x1  = countlines(x0 +'Models',x1);
x1  = countlines(x0 +'Utils', x1);
x1  = countlines(x0 +'ViewModels',x1);
x1  = countlines(x0 +'Data',x1);
x1  = countlines(x0 +'Views\Data',x1);
x1  = countlines(x0 +'Views\DataAttributes',x1);
x1  = countlines(x0 +'Views\Home',x1);
x1  = countlines(x0 +'Views\RecordTree',x1);
x1  = countlines(x0 +'Views\Testdata',x1);
x1  = countlines(x0 +'Views\TestDataRun',x1);
x1  = countlines(x0 +'Views\TestRunning',x1);
x1  = countlines(x0 +'Views\ThinClient',x1);



