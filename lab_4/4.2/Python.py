
import re
import tkinter as tk
from tkinter import ttk, messagebox, scrolledtext

PATTERN = re.compile(r"\b(?:https?://)?(?:www\.)?[A-Za-z0-9-]+(?:\.[A-Za-z0-9-]+)*\.cv\.ua(?:/[^\s]*)?\b", re.IGNORECASE)

class CVFinderApp(tk.Tk):
    def __init__(self):
        super().__init__()
        self.title("Знаходження адрес .cv.ua — Лабораторна")
        self.geometry("900x600")

        #побудова інтерфейсу
        top = ttk.Frame(self)
        top.pack(fill=tk.BOTH, expand=True, padx=8, pady=8)

        lbl = ttk.Label(top, text="Введіть текст (або вставте):")
        lbl.pack(anchor=tk.W)

        self.text = scrolledtext.ScrolledText(top, wrap=tk.WORD, height=12)
        self.text.pack(fill=tk.BOTH, expand=True)

        #кнопка пошуку та лічильник
        mid = ttk.Frame(self)
        mid.pack(fill=tk.X, padx=8, pady=6)

        self.find_btn = ttk.Button(mid, text="Знайти адреси .cv.ua", command=self.find_matches)
        self.find_btn.grid(row=0, column=0, padx=4)

        self.count_var = tk.StringVar(value="Знайдено: 0")
        count_lbl = ttk.Label(mid, textvariable=self.count_var)
        count_lbl.grid(row=0, column=1, padx=8)
        
        #список знайдених адрес 
        right = ttk.Frame(self)
        right.pack(fill=tk.BOTH, expand=True, padx=8, pady=6)

        lb_label = ttk.Label(right, text="Знайдені підтексти (натисніть щоб вибрати):")
        lb_label.pack(anchor=tk.W)

        self.match_list = tk.Listbox(right, height=8)
        self.match_list.pack(fill=tk.BOTH, expand=True)
        self.match_list.bind('<<ListboxSelect>>', self.on_select)
        
        #кнопки для видалення та заміни
        bot = ttk.Frame(self)
        bot.pack(fill=tk.X, padx=8, pady=8)

        del_btn = ttk.Button(bot, text="Вилучити вибраний підтекст (один)", command=self.delete_selected)
        del_btn.grid(row=0, column=0, padx=4, pady=4)

        del_all_btn = ttk.Button(bot, text="Вилучити всі знайдені", command=self.delete_all)
        del_all_btn.grid(row=0, column=1, padx=4, pady=4)

        repl_lbl = ttk.Label(bot, text="Замінити вибраний на:")
        repl_lbl.grid(row=1, column=0, sticky=tk.W, padx=4)
        self.repl_entry = ttk.Entry(bot, width=40)
        self.repl_entry.grid(row=1, column=1, padx=4, sticky=tk.W)

        repl_btn = ttk.Button(bot, text="Замінити вибраний", command=self.replace_selected)
        repl_btn.grid(row=1, column=2, padx=4)

        repl_all_btn = ttk.Button(bot, text="Замінити всі знайдені", command=self.replace_all)
        repl_all_btn.grid(row=1, column=3, padx=4)

        info = ttk.Label(self, text=("Регулярний вираз: (опціонально http/https, www, піддомени)\n"
                                     "\b(?:https?://)?(?:www\.)?[A-Za-z0-9-]+(?:\.[A-Za-z0-9-]+)*\.cv\.ua(?:/[^\s]*)?\b"), justify=tk.LEFT)
        info.pack(anchor=tk.W, padx=8, pady=(0,8))

    def find_matches(self):
        text = self.text.get('1.0', tk.END)
        matches = PATTERN.findall(text)

        seen = set()
        unique_matches = []
        for m in matches:
            if m.lower() not in seen:
                unique_matches.append(m)
                seen.add(m.lower())

        self.match_list.delete(0, tk.END)
        for m in unique_matches:
            self.match_list.insert(tk.END, m)

        self.count_var.set(f"Знайдено: {len(unique_matches)}")
        if not unique_matches:
            messagebox.showinfo("Результат", "Адреси .cv.ua не знайдені у тексті.")

    def on_select(self, event):
        sel = self.get_selected_match()
        if sel:
           
            text = self.text.get('1.0', tk.END)
            idx = text.lower().find(sel.lower())
            if idx >= 0:
                
                row = text.count('\n', 0, idx) + 1
                col = idx - text.rfind('\n', 0, idx)
                pos = f"{row}.{col}"
                self.text.tag_remove('selmatch', '1.0', tk.END)
                self.text.tag_add('selmatch', pos, f"{pos}+{len(sel)}c")
                self.text.tag_config('selmatch', background='yellow')

    def get_selected_match(self):
        sel = self.match_list.curselection()
        if not sel:
            return None
        return self.match_list.get(sel[0])

    def delete_selected(self):
        sel = self.get_selected_match()
        if not sel:
            messagebox.showwarning("Увага", "Спочатку оберіть підтекст у списку.")
            return
        text = self.text.get('1.0', tk.END)
        
        new_text, count = re.subn(re.escape(sel), '', text, count=1, flags=re.IGNORECASE)
        if count == 0:
            messagebox.showinfo("Інфо", "Вибраний підтекст не знайдений у тексті (ймовірно він був змінений).")
            return
        self.text.delete('1.0', tk.END)
        self.text.insert('1.0', new_text)
        messagebox.showinfo("Готово", "Вилучено вибраний підтекст.")
        self.find_matches()

    def delete_all(self):
        matches = self.match_list.get(0, tk.END)
        if not matches:
            messagebox.showwarning("Увага", "Немає знайдених підтекстів для видалення.")
            return
        text = self.text.get('1.0', tk.END)
        for m in matches:
            text = re.sub(re.escape(m), '', text, flags=re.IGNORECASE)
        self.text.delete('1.0', tk.END)
        self.text.insert('1.0', text)
        messagebox.showinfo("Готово", "Вилучено всі знайдені підтексти.")
        self.find_matches()

    def replace_selected(self):
        sel = self.get_selected_match()
        repl = self.repl_entry.get()
        if not sel:
            messagebox.showwarning("Увага", "Оберіть підтекст у списку для заміни.")
            return
        if repl == '':
            messagebox.showwarning("Увага", "Введіть значення для заміни.")
            return
        text = self.text.get('1.0', tk.END)
        new_text, count = re.subn(re.escape(sel), repl, text, count=1, flags=re.IGNORECASE)
        if count == 0:
            messagebox.showinfo("Інфо", "Підтекст не знайдено у тексті.")
            return
        self.text.delete('1.0', tk.END)
        self.text.insert('1.0', new_text)
        messagebox.showinfo("Готово", f"Заміна виконана ({count} разів для першого входження).")
        self.find_matches()

    def replace_all(self):
        matches = self.match_list.get(0, tk.END)
        repl = self.repl_entry.get()
        if not matches:
            messagebox.showwarning("Увага", "Немає знайдених підтекстів для заміни.")
            return
        if repl == '':
            messagebox.showwarning("Увага", "Введіть значення для заміни.")
            return
        text = self.text.get('1.0', tk.END)
        for m in matches:
            text = re.sub(re.escape(m), repl, text, flags=re.IGNORECASE)
        self.text.delete('1.0', tk.END)
        self.text.insert('1.0', text)
        messagebox.showinfo("Готово", "Всі знайдені підтексти замінено.")
        self.find_matches()

if __name__ == '__main__':
    app = CVFinderApp()
    app.mainloop()
