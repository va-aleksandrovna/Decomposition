Необходимо разложить натуральное число типа uint64 на простые множители (далее - разложение). 
Оформить решение в виде класса, который позволяет хранить разложение, вычислять разложение числа (и наоборот, по разложению вычислять число -- для проверки), генерировать строку, которая в читаемом виде содержит разложение и т.д.
Далее, необходимо написать программу, которая открывает текстовый файл (имя файла задавать из командной строки) и последовательно по одному считывает из файла числа, для которых и вычисляет разложение, складывая результат в выходной файл (имя также задается как параметр командной строки).
Считывать по одному и сразу складывать ответ в выходной файл - это требование, то есть нельзя открыть входной файл, считать все числа (в массив), найти разложение всех и скинуть все ответы разом в выходной файл. Предполагайте, что входной файл может содержать очень много чисел, которые не влезут в оперативную память.

Примечание: можно использовать простые (т.н. наивные) алгоритмы для поиска разложения, но можно получить больше опыта, удовольствия и баллов, если использовать более эффективные способы. Но максимум баллов можно получить и используя наивный алгоритм.
Прежде всего оценивается не работоспособность программы, а корректность кода и архитектуры.

Все вычисления происходят в отдельном потоке. Запрещается создавать новый поток для каждого числа. То есть нельзя просто запустить вычисление одного разложения в потоке, затем подождать, когда он закончит вычисления, а потом для следующего числа запускать новый поток.
1.	Во время работы, если ввести с клавиатуры слово exit, то программа ожидает окончания расчета одного (текущего) разложения, затем закрывает выходной и входной файлы и корректно завершает работу.
2.	Если ввести слово pause, то программа приостанавливает работу и закрывает выходной файл. После чего, если ввести resume, то она продолжает свою работу (выходной файл при этом не перезаписывает, а начинает дополнять).
3.	Оптимизировать скорость вычислений, задействуя несколько вычислительных потоков (количество потоков задается пользователем из командной строки: 1, 2, 3, ..., 8). Стратегия может быть следующей: при считывании очередного входного числа, оно передается свободному потоку или потоку с наименьшим размером очереди.
4.	Устроить архитектуру таким образом, чтобы можно было использовать все наработки без изменений для любых похожих задач.
