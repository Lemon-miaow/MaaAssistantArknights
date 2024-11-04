// <copyright file="RecruitTask.cs" company="MaaAssistantArknights">
// MaaWpfGui - A part of the MaaCoreArknights project
// Copyright (C) 2021 MistEO and Contributors
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License v3.0 only as published by
// the Free Software Foundation, either version 3 of the License, or
// any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY
// </copyright>

#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaWpfGui.Configuration.MaaTask;

/// <summary>
/// ����
/// </summary>
public class RecruitTask : BaseTask
{
    public RecruitTask() => TaskType = TaskType.Recruit;

    /// <summary>
    /// Gets or sets a value indicating whether �Ƿ�ʹ�ù��м��پ�
    /// </summary>
    [JsonIgnore]
    public bool UseExpedited { get; set; }

    /// <summary>
    /// Gets or sets ��������д���
    /// </summary>
    public int MaxTimes { get; set; } = 4;

    /// <summary>
    /// Gets or sets a value indicating whether ����Ƹ�����Ȼˢ��
    /// </summary>
    public bool ForceRefresh { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether ���Զ�ȷ��1��
    /// </summary>
    public bool NotChooseLevel1 { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether �Զ�ȷ��3��
    /// </summary>
    public bool ChooseLevel3 { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether �Զ�ȷ��4��
    /// </summary>
    public bool ChooseLevel4 { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether �Զ�ȷ��5��
    /// </summary>
    public bool ChooseLevel5 { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether 3��ʹ��01:00
    /// TODO ������? / ö��
    /// </summary>
    public bool Level3Time0100 { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether 3��ʹ��07:40
    /// TODO ������?
    /// </summary>
    public bool Level3Time0740 { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether ������Tagʱ�Ƿ�ˢ��3��
    /// TODO ������?
    /// </summary>
    public bool RefreshLevel3 { get; set; } = true;

    /// <summary>
    /// Gets or sets 3����ѡTag
    /// </summary>
    public string? Level3FirstTags { get; set; }

    /// <summary>
    /// Gets or sets ��Tag����
    /// TODO ��ö�٣�
    /// </summary>
    public int ExtraTagStrategy { get; set; }

    public override JObject SerializeJsonTask()
    {
        var firstList = (Level3FirstTags ?? string.Empty).Split(';', '��').Select(s => s.Trim());

        var selectList = new List<int>();
        var confirmList = new List<int>();

        if (ChooseLevel3)
        {
            confirmList.Add(3);
        }

        if (ChooseLevel4)
        {
            selectList.Add(4);
            confirmList.Add(4);
        }

        // ReSharper disable once InvertIf
        if (ChooseLevel5)
        {
            selectList.Add(5);
            confirmList.Add(5);
        }

        var taskParams = new JObject
        {
            ["refresh"] = RefreshLevel3,
            ["force_refresh"] = ForceRefresh,
            ["select"] = new JArray(selectList.ToArray()),
            ["confirm"] = new JArray(confirmList.ToArray()),
            ["times"] = MaxTimes,
            ["set_time"] = true,
            ["expedite"] = UseExpedited,
            ["extra_tags_mode"] = ExtraTagStrategy,
            ["expedite_times"] = MaxTimes,
            ["skip_robot"] = NotChooseLevel1,
            ["first_tags"] = new JArray(firstList.ToArray()),
        };

        if (Level3Time0740)
        {
            taskParams["recruitment_time"] = new JObject
            {
                ["3"] = new TimeSpan(7, 40, 0).TotalMinutes, // 7:40
            };
        }
        else if (Level3Time0100)
        {
            taskParams["recruitment_time"] = new JObject
            {
                ["3"] = new TimeSpan(0, 60, 0).TotalMinutes, // 1:00
            };
        }

        taskParams["report_to_penguin"] = Instances.SettingsViewModel.EnablePenguin;
        taskParams["report_to_yituliu"] = Instances.SettingsViewModel.EnableYituliu;
        taskParams["penguin_id"] = Instances.SettingsViewModel.PenguinId;
        taskParams["server"] = Instances.SettingsViewModel.ServerType;

        return taskParams;
    }
}
