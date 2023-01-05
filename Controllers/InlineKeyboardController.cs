using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using UtilityBot.Services;

namespace UtilityBot.Controllers
{
    public class InlineKeyboardController
    {
        private readonly IStorage _memoryStorage;
        private readonly ITelegramBotClient _telegramClient;

        public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }
        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            if (callbackQuery?.Data == null)
                return;

            // Обновление пользовательской сессии новыми данными
            switch (callbackQuery.Data)
            {
                case "length":

                    _memoryStorage.GetSession(callbackQuery.From.Id).SumMode = false;
                    break;

                case "sum":

                    _memoryStorage.GetSession(callbackQuery.From.Id).SumMode = true;
                    break;

                default:

                    _memoryStorage.GetSession(callbackQuery.From.Id).SumMode = false;
                    break;
            }


            // Генерим информационное сообщение
            string modeText = callbackQuery.Data switch
            {
                "length" => " Длина сообщения",
                "sum" => " Сумма чисел",
                _ => String.Empty
            };

            // Отправляем в ответ уведомление о выборе
            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                $"<b>Вы выбрали - {modeText}.{Environment.NewLine}</b>" +
                $"{Environment.NewLine}Можно поменять в главном меню(/start).", cancellationToken: ct, parseMode: ParseMode.Html);
        }
    }
}