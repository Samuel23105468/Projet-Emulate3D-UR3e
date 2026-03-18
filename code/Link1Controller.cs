using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.IO;
using Demo3D.Native;
using Demo3D.Visuals;
using Demo3D.Components;
using Demo3D.Common.Random;

[Auto] public class Link1Controller : NativeObject {
    public Link1Controller(Visual sender) : base(sender) {}

    string filePath = @"C:\Users\Etudiant\Desktop\M2_IN\Projet Jumeau numérique\angles_robot.csv";

    [Auto] void OnReset(Visual sender) {
        sender.FindAspect<PositionController>().TargetPosition = 0;
    }

    [Auto] IEnumerable OnInitialize (Visual sender)
    {
        var PosAspect = sender.FindAspect<PositionController>();

        while (true) 
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (StreamReader sr = new StreamReader(fs))
                {
                    var lines = new List<string>();

                    while (!sr.EndOfStream)
                        lines.Add(sr.ReadLine());

                    if (lines.Count > 1)
                    {
                        var data = lines[1].Split(',');

                        int angle = Convert.ToInt32(data[1]);

                        PosAspect.TargetPosition = angle;
                    }
                }
            }
            catch
            {
                // ignore si Python écrit au même moment
            }

            yield return Wait.UntilTrue(() => PosAspect.AtTargetPosition, sender);
            yield return Wait.ForSeconds(0.1);
        }
    }
}
